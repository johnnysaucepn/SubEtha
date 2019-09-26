using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Howatworks.Thumb.Matrix
{
    public class MatrixFormsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MatrixApplicationContext>().AsSelf().SingleInstance();
        }
    }
}
