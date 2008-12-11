namespace NUnit.UiException.Controls
{
    partial class CodeBoxComposite
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this._codeBox = new NUnit.UiException.Controls.CodeBox();
            this._btnLeft = new NUnit.UiException.Controls.HoverButton();
            this._btnUp = new NUnit.UiException.Controls.HoverButton();
            this._btnRight = new NUnit.UiException.Controls.HoverButton();
            this._btnDown = new NUnit.UiException.Controls.HoverButton();
            this.SuspendLayout();
            // 
            // _codeBox
            // 
            this._codeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._codeBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._codeBox.HighlightedLine = 0;
            this._codeBox.Location = new System.Drawing.Point(8, 8);
            this._codeBox.MouseWheelDistance = 20;
            this._codeBox.Name = "_codeBox";
            this._codeBox.Size = new System.Drawing.Size(480, 250);
            this._codeBox.TabIndex = 9;
            // 
            // _btnLeft
            // 
            this._btnLeft.Direction = NUnit.UiException.Controls.HoverButtonDirection.Left;
            this._btnLeft.Location = new System.Drawing.Point(0, 8);
            this._btnLeft.Enabled = true;
            this._btnLeft.Name = "_btnLeft";
            this._btnLeft.Size = new System.Drawing.Size(8, 250);
            this._btnLeft.TabIndex = 8;
            // 
            // _btnUp
            // 
            this._btnUp.Direction = NUnit.UiException.Controls.HoverButtonDirection.Up;
            this._btnUp.Location = new System.Drawing.Point(8, 0);
            this._btnUp.Enabled = true;
            this._btnUp.Name = "_btnUp";
            this._btnUp.Size = new System.Drawing.Size(480, 8);
            this._btnUp.TabIndex = 7;
            // 
            // _btnRight
            // 
            this._btnRight.Direction = NUnit.UiException.Controls.HoverButtonDirection.Right;
            this._btnRight.Location = new System.Drawing.Point(488, 8);
            this._btnRight.Enabled = true;
            this._btnRight.Name = "_btnRight";
            this._btnRight.Size = new System.Drawing.Size(8, 250);
            this._btnRight.TabIndex = 6;
            // 
            // _btnDown
            // 
            this._btnDown.Direction = NUnit.UiException.Controls.HoverButtonDirection.Down;
            this._btnDown.Location = new System.Drawing.Point(8, 258);
            this._btnDown.Enabled = true;
            this._btnDown.Name = "_btnDown";
            this._btnDown.Size = new System.Drawing.Size(480, 8);
            this._btnDown.TabIndex = 5;
            // 
            // ScrollableCodeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._codeBox);
            this.Controls.Add(this._btnLeft);
            this.Controls.Add(this._btnUp);
            this.Controls.Add(this._btnRight);
            this.Controls.Add(this._btnDown);
            this.Name = "ScrollableCodeBox";
            this.Size = new System.Drawing.Size(496, 266);
            this.ResumeLayout(false);

        }

        #endregion

        private HoverButton _btnDown;
        private HoverButton _btnRight;
        private HoverButton _btnUp;
        private HoverButton _btnLeft;
        private CodeBox _codeBox;
    }
}
