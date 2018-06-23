using RayRender.Core;
using RayRender.Filters;
using RayRender.Images;
using RayRender.Inputs;
using RayRender.Interfaces;
using RayRender.Maths;
using RayRender.Outputs;
using RayRender.Rays;
using RayRender.Shapes;
using RayRender.Utils;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayRender.Forms
{
    public partial class RayRenderForm : Form
    {

        public RayRenderForm()
        {
            InitializeComponent();
            Logger.Inicialize();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.btExecute.Enabled = false;
            this.btSelectFile.Enabled = false;
            this.txtFileName.Enabled = false;
            this.lbElapsedTime.Text = "Redering...";

            Stopwatch watch = Stopwatch.StartNew();

            Task renderTask = Task.Run(() =>
            {
                IWorld world = new World();

                world.Image = Image.LoadFromFile(this.txtFileName.Text);

                ConvolutionFilter filter = new ConvolutionFilter();

                filter.Factor = 1.0f;
                filter.Size = 3;
                filter.Kernel = new float[3, 3]{ { -1, -1, -1, },
                            { -1,  8, -1, },
                            { -1, -1, -1, }, };


                filter.Execute(world);

                //world.GetStage<Input>(0).Reader = new SceneReader(this.txtFileName.Text);

                //world.Execute();
            });

            TaskAwaiter awaiter = renderTask.GetAwaiter();

            awaiter.OnCompleted(() =>
            {
                watch.Stop();

                this.btExecute.Enabled = true;
                this.btSelectFile.Enabled = true;
                this.txtFileName.Enabled = true;

                this.lbElapsedTime.Text = string.Format("Elapsed {0} milliseconds", watch.ElapsedMilliseconds);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtFileName.Text = this.openFileDialog1.FileName;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
