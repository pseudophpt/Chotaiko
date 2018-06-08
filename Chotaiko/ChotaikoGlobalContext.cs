using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chotaiko.Game;

namespace Chotaiko
{
    static class ChotaikoGlobalContext
    {
        static ChotaikoGameContext CurrentGameContext;
        static ChotaikoKey[] Keys;

        public static void SetKeys(ChotaikoKey Key1, ChotaikoKey Key2, ChotaikoKey Key3, ChotaikoKey Key4)
        {
            Keys = new ChotaikoKey[4];
            Keys[0] = Key1;
            Keys[1] = Key2;
            Keys[2] = Key3;
            Keys[3] = Key4;
        }

        public static void SetGameContext(ChotaikoGameContext Context)
        {
            CurrentGameContext = Context;
        }

        public static ChotaikoGameContext GetGameContext()
        {
            return CurrentGameContext;
        }

        public static void DrawGameContext()
        {
            CurrentGameContext.Draw();
        }

        public static void UpdateGameContext()
        {
            CurrentGameContext.Update();
        }

        public static int OnPress(ChotaikoKey Key)
        {
            return CurrentGameContext.OnPress(Key);
        }
    }
}
