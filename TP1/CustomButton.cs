using System.Drawing;
using System.Windows.Forms;

namespace TP1
{
    /// <summary>
    /// Classe personnalisée héritant de Button pour afficher le nombre de bombes adjacentes sur le bouton.
    /// </summary>
    public class CustomButton : Button
    {
        /// <summary>
        /// Stocke le nombre de bombes adjacentes à cette case.
        /// </summary>
        public int BombCount { get; set; }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Si le bouton est désactivé, redessiner manuellement le texte.
            if (!this.Enabled)
            {
                // Dessiner l'arrière-plan.
                pevent.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

                // Dessiner le texte centré.
                if (this.BombCount > 0)
                {
                    Color textColor = GetTextColorByBombCount(BombCount);
                    TextRenderer.DrawText(pevent.Graphics, this.BombCount.ToString(), this.Font, this.ClientRectangle, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        // Renvoie la couleur du texte selon le nombre de bombes adjacentes.
        private Color GetTextColorByBombCount(int bombCount)
        {
            switch (bombCount)
            {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Purple;
                default:
                    return Color.Pink;
            }
        }
    }
}
