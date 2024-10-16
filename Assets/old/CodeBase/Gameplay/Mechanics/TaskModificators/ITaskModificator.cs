﻿using CodeBase.Gameplay.Mechanics.Root;

namespace CodeBase.Gameplay.Mechanics
{
    public interface ITaskModificator
    {
        public void Initialize(Task task);

        public void Dispose();
    }
}