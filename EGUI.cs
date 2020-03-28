using Ethereal.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace Ethereal
{
    /* Defines Variables and Methods for Ethereal GUI Elements */
    public class EGUI
    {
        public bool GUIActive { get; set; }
        public static List<EGUI> GUIElements { get; set; } = new List<EGUI>();
        public virtual bool RemoveOnClose => false;

        public EGUI()
        {
            GUIElements.Add(this);
        }

        public List<InterfaceButton> Buttons { get; set; } = new List<InterfaceButton>();

        public InterfaceButton AddButton(Func<Rectangle> position, Action<Player> pressAction)
        {
            InterfaceButton button = new InterfaceButton(position, pressAction);
            Buttons.Add(button);
            return button;
        }

        public InterfaceButton AddButton(Func<Rectangle> position, Action<Player> pressAction, Action<Player, SpriteBatch> hoverAction)
        {
            InterfaceButton button = new InterfaceButton(position, pressAction, hoverAction);
            Buttons.Add(button);
            return button;
        }

        public void CloseGUI()
        {
            OnClose();
            GUIActive = false;
            if (RemoveOnClose) GUIElements.Remove(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Player player)
        {
            PostDraw(spriteBatch, player);

            foreach (InterfaceButton button in Buttons)
                button.Update(spriteBatch, player);
        }

        public virtual void OnClose()
        {
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, Player player)
        {
        }

        public virtual bool PreDraw()
        {
            return GUIActive;
        }

        public void RemoveButton(InterfaceButton button)
        {
            Buttons.Remove(button);
        }
    }
}