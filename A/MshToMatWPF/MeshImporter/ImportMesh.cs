using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Drawing;

namespace MshToMatWPF.MeshImporter
{
    abstract class ImportMesh
    {
        protected string filename { get; private set; }

        public MeshContainer geomManager { get; protected set; }

        public ImportMesh(string filename)
        {
            this.filename = filename;
            geomManager = null;
        }

        public abstract void Build();
    }
}
