// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

namespace Artics.Physics.UnityPhisicsVisualizers.Base
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
