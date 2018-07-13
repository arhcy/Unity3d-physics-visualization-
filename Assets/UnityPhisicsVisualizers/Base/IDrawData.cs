using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    public interface IDrawData
    {
        /// <summary>
        /// draw data with stored params
        /// </summary>
        void Draw();

        /// <summary>
        /// draw data with custom color
        /// </summary>
        /// <param name="color"></param>
        void Draw(UnityEngine.Color color);
    }

    

}
