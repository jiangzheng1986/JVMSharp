namespace JVMSharp
{
    public struct Color
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public Color(float R, float G, float B, float A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", R.ToString(), G.ToString(), B.ToString(), A.ToString());
        }

        public override bool Equals(object Obj)
        {
            if (Obj is Color)
            {
                Color Other = (Color)Obj;
                return Equals(Other);
            }

            return false;
        }

        public bool Equals(Color Other)
        {
            return R == Other.R && G == Other.G && B == Other.B && A == Other.A;
        }

        public static bool operator ==(Color Color1, Color Color2)
        {
            return Color1.Equals(Color2);
        }

        public static bool operator !=(Color Color1, Color Color2)
        {
            return !Color1.Equals(Color2);
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode() ^ A.GetHashCode();
        }

        public static Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        public static Color FromRGBA(int R, int G, int B, int A)
        {
            const float Inv255 = 1.0f / 255.0f;
            return new Color(R * Inv255, G * Inv255, B * Inv255, A * Inv255);
        }

        public static Color FromRGB(int R, int G, int B)
        {
            return FromRGBA(R, G, B, 255);
        }

        public static Color FromDword(uint Color)
        {
            const float Inv255 = 1.0f / 255.0f;
            float R = (Color & 0xff) * Inv255;
            float G = ((Color >> 8) & 0xff) * Inv255;
            float B = ((Color >> 16) & 0xff) * Inv255;
            float A = (Color >> 24) * Inv255;
            return new Color(R, G, B, A);
        }

        public uint ToDword()
        {
            byte ByteR = (byte)(R * 255);
            byte ByteG = (byte)(G * 255);
            byte ByteB = (byte)(B * 255);
            byte ByteA = (byte)(A * 255);
            uint Dword = (uint)((ByteR & 0xff) | ((ByteG & 0xff) << 8) | ((ByteB & 0xff) << 16) | (ByteA & 0xff) << 24);
            return Dword;
        }

        public static uint RGBAToDword(int R, int G, int B, int A)
        {
            uint Dword = (uint)((R & 0xff) | ((G & 0xff) << 8) | ((B & 0xff) << 16) | (A & 0xff) << 24);
            return Dword;
        }

        public static Color EDITOR_UI_COLOR_KEY = FromRGBA(255, 0, 255, 255);

        public static Color EDITOR_UI_COLOR_WHITE = FromRGBA(255, 255, 255, 255);

        public static Color EDITOR_UI_GENERAL_BACK_COLOR = FromRGBA(60, 63, 65, 255);

        public static Color EDITOR_UI_GENERAL_TEXT_COLOR = FromRGBA(255, 255, 255, 255);

        public static Color EDITOR_UI_GRAY_TEXT_COLOR = FromRGBA(153, 153, 153, 255);

        public static Color EDITOR_UI_LIGHT_TEXT_COLOR = FromRGBA(241, 241, 241, 255);

        public static Color EDITOR_UI_BAR_COLOR = FromRGBA(76, 76, 78, 255);

        public static Color EDITOR_UI_CONTROL_BACK_COLOR = FromRGBA(43, 44, 46, 255);

        public static Color EDITOR_UI_HILIGHT_COLOR_GRAY = FromRGBA(81, 82, 84, 255);

        public static Color EDITOR_UI_HILIGHT_COLOR_BLUE = FromRGBA(40, 154, 236, 255);

        public static Color EDITOR_UI_HILIGHT_COLOR_ORANGE = FromRGBA(214, 112, 61, 255);

        public static Color EDITOR_UI_HILIGHT_COLOR_GREEN = FromRGBA(0, 130, 100, 255);

        public static Color EDITOR_UI_ACTIVE_TOOL_OR_MENU = EDITOR_UI_HILIGHT_COLOR_BLUE;

        public static Color EDITOR_UI_DISABLE_TEXT_COLOR = FromRGBA(51, 51, 51, 255);

        public static Color EDITOR_UI_GRAY_DRAW_COLOR = FromRGBA(128, 128, 128, 128);

        public static Color EDITOR_UI_MENU_BACK_COLOR = FromRGBA(47, 47, 48, 255);

        public static Color EDITOR_UI_MENU_HILIGHT_COLOR = FromRGBA(71, 71, 72, 255);

        public static Color TREE_ITEM_HILIGHT_COLOR_GRAY = FromRGBA(63, 63, 70, 255);

        public static Color TREE_ITEM_HILIGHT_COLOR_BLUE = FromRGBA(61, 163, 255, 255);
    }
}
