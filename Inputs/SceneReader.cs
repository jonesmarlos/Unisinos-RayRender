using RayRender.Core;
using RayRender.Images;
using RayRender.Interfaces;
using RayRender.Filters;
using RayRender.Lights;
using RayRender.Outputs;
using RayRender.Renders;
using RayRender.Shapes;
using RayRender.Stages;
using RayRender.Utils;
using System.Linq;
using System.Text;

namespace RayRender.Inputs
{
    public enum Token
    {
        Nothing,
        Empty,
        Comment,
        Attribute,
        BeginElement,
        EndElement,
        Camera,
        NumberOfLights,
        NumberOfObjects,
        ProgramRender,
        NumberOfFilters,
        Output
    }

    public class SceneReader : IReader
    {
        private string fileName;
        private string[] lines;
        private int currentLine;

        public SceneReader(string fileName)
        {
            this.fileName = fileName;
            this.lines = new string[0];
            this.currentLine = -1;
        }

        public SceneReader() : this(string.Empty)
        {

        }

        private bool HasNextLine()
        {
            return (currentLine < lines.Length);
        }

        private string NextLine()
        {
            while (this.HasNextLine())
            {
                this.currentLine++;

                if (this.currentLine >= this.lines.Length)
                {
                    return string.Empty;
                }

                string line = this.lines[this.currentLine];
                if (!string.IsNullOrEmpty(line))
                {
                    return line.Trim();
                }
            }

            return string.Empty;
        }

        private Token GetToken(string line, Parameters parameters)
        {
            parameters.Clear();

            if (string.IsNullOrEmpty(line))
            {
                return Token.Empty;
            }

            if (line.ElementAt<char>(0) == '#')
            {
                return Token.Comment;
            }

            string[] p = line.Split(' ');

            if (p.Length == 0)
            {
                return Token.Nothing;
            }

            Token token = Token.Nothing;

            if (p[0].Last<char>() == ':')
            {
                p[0] = p[0].TrimEnd(':');
            }

            switch (p[0])
            {
                case "begin":
                    token = Token.BeginElement;
                    break;
                case "end":
                    token = Token.EndElement;
                    break;
                case "cam":
                    token = Token.Camera;
                    break;
                case "nlights":
                    token = Token.NumberOfLights;
                    break;
                case "nobjetcs":
                    token = Token.NumberOfObjects;
                    break;
                case "nfilters":
                    token = Token.NumberOfFilters;
                    break;
                case "program":
                    token = Token.ProgramRender;
                    break;
                case "output":
                    token = Token.Output;
                    break;
                default:
                    token = Token.Attribute;
                    break;

            }

            int startIndex = 1;

            if (token == Token.Attribute)
            {
                startIndex = 0;
            }

            for (int index = startIndex; index < p.Length; index++)
            {
                parameters.Add(p[index]);
            }

            return token;
        }

        public void Execute(IWorld parameter)
        {
            this.lines = System.IO.File.ReadAllLines(this.fileName, Encoding.UTF8);
            this.currentLine = -1;

            int currentStage = -1;
            int currentLight = -1;
            int currentObject = -1;
            int currentFilter = -1;
            int numberOfLights = -1;
            int numberOfObjects = -1;
            int numberOfFilters = -1;
            int lastStage = -1;
            int lastLight = -1;
            int lastObject = -1;
            int lastFilter = -1;

            while (this.HasNextLine())
            {
                string line = this.NextLine();
                Parameters parameters = new Parameters();

                Token token = this.GetToken(line, parameters);

                switch (token)
                {
                    case Token.Nothing:
                        return;
                    case Token.Empty:
                        continue;
                    case Token.Comment:
                        Logger.Debug("Ignore comment: {0}", line);
                        break;
                    case Token.BeginElement:
                        string beginElementName = parameters.GetName();

                        switch (beginElementName)
                        {
                            case "stage":
                                int beginState = parameters.GetInt(1);

                                if (currentStage == -1)
                                {
                                    currentStage = beginState;

                                    Logger.Debug("BeginState({0})", beginState);
                                }

                                break;
                            case "light":
                                int beginLight = parameters.GetInt(1);

                                if (currentLight == -1)
                                {
                                    currentLight = beginLight;
                                    string lightType = parameters.GetString(2);

                                    if (lightType == "ambient")
                                    {
                                        parameter.Lights.Insert(currentLight, new Light(LightType.AmbientLight));
                                    }
                                    else
                                    {
                                        parameter.Lights.Insert(currentLight, new Light(LightType.PointLight));
                                    }

                                    Logger.Debug("BeginLight({0})", beginLight);
                                }

                                break;
                            case "shape":
                                int beginShape = parameters.GetInt(1);

                                if (currentObject == -1)
                                {
                                    currentObject = beginShape;
                                    string objectType = parameters.GetString(2);

                                    IShape shape = ShapeFactory.Create(objectType);

                                    parameter.Shapes.Insert(currentObject, shape);

                                    Logger.Debug("BeginShape({0})", beginShape);
                                }

                                break;
                            case "filter":
                                int beginFilter = parameters.GetInt(1);

                                if (currentFilter == -1)
                                {
                                    currentFilter = beginFilter;
                                    string filterType = parameters.GetString(2);

                                    IFilter filter = FilterFactory.Create(filterType);

                                    parameter.GetStage<PosProcessing>(currentStage).Filters.Insert(currentFilter, filter);

                                    Logger.Debug("BeginFilter({0})", beginFilter);
                                }

                                break;
                        }

                        break;
                    case Token.EndElement:
                        string endElementName = parameters.GetName();

                        switch (endElementName)
                        {
                            case "stage":
                                int endStage = parameters.GetInt(1);

                                if (endStage == currentStage)
                                {
                                    lastStage = currentStage;
                                    currentStage = -1;

                                    Logger.Debug("EndState({0})", endStage);
                                }

                                break;
                            case "light":
                                int endLight = parameters.GetInt(1);

                                if (endLight == currentLight)
                                {
                                    lastLight = currentLight;
                                    currentLight = -1;

                                    Logger.Debug("EndLight({0})", endLight);
                                }

                                break;
                            case "shape":
                                int endShape = parameters.GetInt(1);

                                if (endShape == currentObject)
                                {
                                    lastObject = currentObject;
                                    currentObject = -1;

                                    Logger.Debug("EndShape({0})", endShape);
                                }

                                break;
                            case "filter":
                                int endFilter = parameters.GetInt(1);

                                if (endFilter == currentFilter)
                                {
                                    lastFilter = currentFilter;
                                    currentFilter = -1;

                                    Logger.Debug("EndFilter({0})", endFilter);
                                }

                                break;
                        }

                        break;
                    case Token.Camera:
                        IVector eye = parameters.GetVector(0);

                        IVector direction = parameters.GetVector(3);

                        parameter.Camera = new Camera(eye, direction);

                        Logger.Debug("Camera({0}, {1})", eye, direction);
                        break;
                    case Token.NumberOfLights:
                        numberOfLights = parameters.GetInt(0);

                        Logger.Debug("Number of Lights: {0}", numberOfLights);
                        break;
                    case Token.NumberOfObjects:
                        numberOfObjects = parameters.GetInt(0);

                        Logger.Debug("Number of Objects: {0}", numberOfObjects);
                        break;
                    case Token.Attribute:

                        if (currentLight >= 0)
                        {
                            ILight light = parameter.Lights.ElementAt<ILight>(currentLight);

                            light.Parse(parameters);
                        }
                        else if (currentObject >= 0)
                        {
                            IShape shape = parameter.Shapes.ElementAt<IShape>(currentObject);

                            shape.Parse(parameters);

                        }
                        else if (currentFilter >= 0)
                        {
                            IFilter filter = parameter.GetStage<PosProcessing>(currentStage).Filters.ElementAt<IFilter>(currentFilter);

                            filter.Parse(parameters);
                        }
                        else if (currentStage == 2)
                        {
                            parameter.GetStage<Render>(currentStage).RayRender.Parse(parameters);
                        }

                        break;
                    case Token.ProgramRender:
                        string renderProgram = parameters.GetString(0);

                        parameter.GetStage<Render>(currentStage).RayRender = new RayTracingRender();

                        break;
                    case Token.NumberOfFilters:
                        numberOfFilters = parameters.GetInt(0);

                        break;
                    case Token.Output:
                        int outputWidth = parameters.GetInt(0);
                        int outputHeight = parameters.GetInt(1);
                        string outputFormat = parameters.GetString(2);

                        parameter.Image = new PixImage(outputWidth, outputHeight);

                        if (outputFormat == "s")
                        {
                            // Output to screen
                        }
                        else
                        {
                            parameter.GetStage<Output>(currentStage).Writer = new SceneWriter(outputFormat);

                            Logger.Debug("Output {0}x{1} to file {2}", outputWidth, outputHeight, outputFormat);
                        }

                        break;
                }
            }
        }
    }
}
