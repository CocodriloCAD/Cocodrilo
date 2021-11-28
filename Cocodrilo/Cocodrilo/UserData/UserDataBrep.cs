using System;
using System.Collections.Generic;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.UserData
{
    [Guid("9CB19B78-3C1E-4A3C-BD17-71427716A0A2")]
    public class UserDataBrep : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }

        public UserDataBrep()
        {
            BrepId = -1;
        }

        #region Rhino methods
        protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        {
            if (source is UserDataBrep src)
            {
                BrepId = src.BrepId;
            }
        }
        #endregion

        #region Read/Write
        public override bool ShouldWrite
        {
            get
            {
                if (BrepId != 0)
                    return true;
                return false;
            }
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = archive.ReadDictionary();

            if (dict.ContainsKey("BrepId"))
            {
                BrepId = (int) dict["BrepId"];
            }

            return true;
        }
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");

            dict.Set("BrepId", BrepId);

            archive.WriteDictionary(dict);

            return true;
        }
        #endregion

    }
}
