using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XZ.Edit.Forms {
    public class ResList {
        public const int NoneIndex = 0;
        public const int ClassIndex = 1;
        public const int ConstIndex = 2;
        public const int DelegateIndex = 3;
        public const int EnumIndex = 4;
        public const int EventIndex = 5;
        public const int InterfaceIndex = 6;
        public const int NameSpaceIndex = 7;
        public const int StructIndex = 8;
        public const int FieldIndex = 9;
        public const int PropertyIndex = 10;
        public const int MethodIndex = 11;
        public const int HPPropertyIndex = 12;

        static ResList() {
            ImageList = new List<Image>();
            ImageList.Add(Resource.none);
            ImageList.Add(Resource._class);
            ImageList.Add(Resource._const);
            ImageList.Add(Resource._delegate);
            ImageList.Add(Resource._enum);
            ImageList.Add(Resource._event);
            ImageList.Add(Resource._interface);
            ImageList.Add(Resource._namespace);
            ImageList.Add(Resource._struct);
            ImageList.Add(Resource.field);
            ImageList.Add(Resource.property);
            ImageList.Add(Resource.method);
            ImageList.Add(Resource.hPProperty);
        }

        public static List<Image> ImageList { get; set; }
    }
}
