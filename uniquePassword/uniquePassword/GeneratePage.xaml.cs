using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace uniquePassword
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GeneratePage : ContentPage
	{
		public GeneratePage ()
		{
			InitializeComponent ();

            lengthDisplayLabel.Text = Convert.ToInt32(lengthSlider.Value).ToString();
        }
        private async void GenerateButtonClicked(object sender, EventArgs e)
        {
            //var pass = MasterPasswordEntry.Text;
            var generatedPass = HashAndReturnPassword();
            var valueSlider = lengthSlider.Value;
            var wishToCopyPassword = await DisplayAlert("Done", $"Your password has been generated \n{generatedPass} of length {valueSlider}", "COPY", "Cancel");
            if (wishToCopyPassword) await Clipboard.SetTextAsync(generatedPass);
        }

        private String HashAndReturnPassword()
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(MasterPasswordEntry.Text+WebsiteNameEntry.Text));
            var passHash = string.Concat(hash.Select(b => b.ToString("x2")));
            var passAfterAppending = "";
            passAfterAppending = (Convert.ToInt32(passHash.Length % lengthSlider.Value) == 0) ? passHash : passHash + new String('0', Convert.ToInt32(lengthSlider.Value - passHash.Length % lengthSlider.Value));
            var lengthToSpitTheHash = Convert.ToInt32(passAfterAppending.Length / lengthSlider.Value);
            passHash = "";
            for(int i=0;i<lengthSlider.Value;i++)
            {
                var splitedHex = passAfterAppending.Substring(i * lengthToSpitTheHash, lengthToSpitTheHash);
                var intConstant = Int64.Parse(splitedHex, System.Globalization.NumberStyles.HexNumber);
                if (IncludeMixCase.IsToggled && i==0)
                {
                    passHash += (char) (intConstant%26+65);
                } else if (IncludeSpecialChar.IsToggled && i == Math.Truncate(lengthSlider.Value/2))
                {
                    passHash += (char) (intConstant % 7 + 91);
                } else if (IncludeNumeric.IsToggled && i == lengthSlider.Value-1)
                {
                    passHash += (char) (intConstant % 10 + 48);
                } else
                {
                    passHash += (char) (intConstant % 26 + 97);
                }
            }

            return passHash;
        }

        private void PasswordToggle_Toggled(object sender, ToggledEventArgs e)
        {
            if(PasswordToggle.IsToggled)
            {
                MasterPasswordEntry.IsPassword = false;
            } else
            {
                MasterPasswordEntry.IsPassword = true;
            }
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1.0);

            lengthSlider.Value = newStep * 1.0;
            lengthDisplayLabel.Text = Convert.ToInt32(newStep).ToString();
        }

        private void EnableOrDisableGenerateButton(object sender, EventArgs e)
        {
            if ((MasterPasswordEntry.Text != null) && !MasterPasswordEntry.Text.Equals("")
                && (WebsiteNameEntry.Text != null) && !WebsiteNameEntry.Text.Equals(""))
            {
                GeneratePasswordButton.IsEnabled = true;
            } else
            {
                GeneratePasswordButton.IsEnabled = false;
            }
        }
    }
}