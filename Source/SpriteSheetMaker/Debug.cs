using System;
using System.Windows.Forms;

namespace SpriteSheetMaker
{
    public static class Debug
    {
        public static void Log(string msg)
        {
            MessageBox.Show(msg, "Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void LogWarning(string msg)
        {
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void LogError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void LogException(Exception ex)
        {
            LogError(ex.ToString());
        }
    }
}