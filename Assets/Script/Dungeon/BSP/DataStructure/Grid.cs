using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructure
{
    public struct Grid
    {
        public Rect gridSize;

        public Grid(Rect p_grid_size) {
            gridSize = p_grid_size;
        }

    }
}