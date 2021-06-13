using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Hexapawn.Models {
    public class Darwin {
        public Darwin() { }

        public int level { get; set; }

        public List<LogicBox> LogicBoxes { get; set; }

        public void InstantiateWithBoxes() {
            LogicBoxes = new List<LogicBox>() {
                /** LOGIC BOXES
                * These are the boxes that darwin uses
                * Naming: b -> check chart - 1
                */

                // Round 2 boxes
                new LogicBox(2, 2, 2, 0), // 0
                new LogicBox(2, 2, 0, 0), // 1

                // Round 4 boxes
                new LogicBox(2, 2, 2, 2), // 2
                new LogicBox(2, 2, 2, 2), // 3
                new LogicBox(2, 2, 2, 0), // 4
                new LogicBox(2, 2, 2, 0), // 5
                new LogicBox(2, 2, 2, 0), // 6
                new LogicBox(2, 0, 2, 0), // 7
                new LogicBox(2, 2, 0, 0), // 8
                new LogicBox(0, 2, 2, 0), // 9
                new LogicBox(2, 0, 2, 0), // 10
                new LogicBox(0, 0, 2, 2), // 11
                new LogicBox(0, 0, 0, 2), // 12

                // Round 6 boxes
                new LogicBox(0, 0, 2, 2), // 13
                new LogicBox(0, 2, 0, 0), // 14
                new LogicBox(0, 0, 2, 2), // 15
                new LogicBox(0, 0, 2, 2), // 16
                new LogicBox(0, 2, 2, 0), // 17
                new LogicBox(2, 0, 0, 2), // 18
                new LogicBox(2, 0, 2, 2), // 19
                new LogicBox(2, 0, 2, 0), // 20
                new LogicBox(0, 2, 2, 0), // 21
                new LogicBox(0, 0, 2, 2), // 22
                new LogicBox(2, 2, 0, 0) // 23
            };
        }
    }
}