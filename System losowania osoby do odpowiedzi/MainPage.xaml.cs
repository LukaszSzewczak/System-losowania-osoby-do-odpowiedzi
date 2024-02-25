using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System_losowania_osoby_do_odpowiedzi
{
    public partial class MainPage : ContentPage
    {
        private List<string> listaUczniow = new List<string>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void DodajUcznia_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                listaUczniow.Add(txtName.Text);
                txtName.Text = string.Empty;
                OdswiezListe();
            }
        }

        private void LosujOsobe_Clicked(object sender, EventArgs e)
        {
            if (listaUczniow.Any())
            {
                Random rnd = new Random();
                string wylosowanaOsoba = listaUczniow[rnd.Next(listaUczniow.Count)];
                DisplayAlert("Wylosowana osoba", wylosowanaOsoba, "OK");
            }
            else
            {
                DisplayAlert("Błąd", "Lista uczniów jest pusta.", "OK");
            }
        }

        private void WczytajListe_Clicked(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "listaUczniow.txt");
            if (File.Exists(filePath))
            {
                listaUczniow = File.ReadAllLines(filePath).ToList();
                OdswiezListe();
            }
            else
            {
                DisplayAlert("Błąd", "Plik nie istnieje.", "OK");
            }
        }

        private void ZapiszListe_Clicked(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "listaUczniow.txt");
            File.WriteAllLines(filePath, listaUczniow);
            DisplayAlert("Sukces", "Lista została zapisana.", "OK");
        }

        private void OdswiezListe()
        {
            lstUczniowie.ItemsSource = null;
            lstUczniowie.ItemsSource = listaUczniow;
        }

        private async void EdytujListe_Clicked(object sender, EventArgs e)
        {
            string selectedUczen = await DisplayActionSheet("Wybierz ucznia do edycji lub usunięcia", "Anuluj", null, listaUczniow.ToArray());

            if (selectedUczen != null && selectedUczen != "Anuluj")
            {
                string akcja = await DisplayActionSheet("Wybierz akcję dla ucznia: " + selectedUczen, "Anuluj", null, "Edytuj", "Usuń");

                if (akcja == "Edytuj")
                {
                    string noweImie = await DisplayPromptAsync("Edytuj ucznia", "Wpisz nowe imię:", placeholder: selectedUczen);

                    if (!string.IsNullOrWhiteSpace(noweImie))
                    {
                        int index = listaUczniow.IndexOf(selectedUczen);
                        listaUczniow[index] = noweImie;
                        OdswiezListe();
                    }
                }
                else if (akcja == "Usuń")
                {
                    listaUczniow.Remove(selectedUczen);
                    OdswiezListe();
                }
            }
        }
    }
}
