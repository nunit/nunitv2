namespace NUnit.UiException.Controls
{
    partial class CodeBoxWindow
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this._codeBoxComposite = new NUnit.UiException.Controls.CodeBoxComposite();
            this.SuspendLayout();
            // 
            // _codeBoxComposite
            // 
            this._codeBoxComposite.BackColor = System.Drawing.Color.White;
            this._codeBoxComposite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._codeBoxComposite.ErrorSource = null;
            this._codeBoxComposite.Location = new System.Drawing.Point(0, 0);
            this._codeBoxComposite.Name = "_codeBoxComposite";
            this._codeBoxComposite.ScrollingDistance = 5;
            this._codeBoxComposite.Size = new System.Drawing.Size(498, 268);
            this._codeBoxComposite.TabIndex = 0;
            // 
            // CodeViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 268);
            this.Controls.Add(this._codeBoxComposite);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CodeViewForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CodeViewForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private CodeBoxComposite _codeBoxComposite;
    }
}