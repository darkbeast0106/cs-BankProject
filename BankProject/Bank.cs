using System.Text.RegularExpressions;

namespace BankProject
{
    public class Bank
    {
        /// <summary>
        /// Bankszámla adatainak a tárolását megvalósító osztály
        /// </summary>
        private class Szamla
        {
            public Szamla(string nev, string szamlaszam)
            {
                Nev = nev;
                Szamlaszam = szamlaszam;
            }

            public string Nev { get; set; }
            public string Szamlaszam { get; set; }
            public ulong Egyenleg { get; set; }
        }
        /// <summary>
        /// A bank számláit tartalmazó lista
        /// </summary>
        private List<Szamla> szamlak = new List<Szamla>();

        /// <summary>
        /// Új számlát nyit a megadott névvel, számlaszámmal, 0 Ft egyenleggel
        /// </summary>
        /// <param name="nev">A számla tulajdonosának neve</param>
        /// <param name="szamlaszam">A számla számlaszáma</param>
        public void UjSzamla(string nev, string szamlaszam)
        {
            if (nev == null)
            {
                throw new ArgumentNullException("A név nem lehet üres"
                    , nameof(nev));
            }
            if (String.IsNullOrEmpty(nev.Trim()))
            {
                throw new ArgumentException("A név nem lehet üres"
                    , nameof(nev));
            }
            if (Regex.IsMatch(nev, @"[^\w\s]"))
            {
                throw new ArgumentException("A név nem tartalmazhat speciális karaktert"
                    , nameof(nev));
            }

            try
            {
                SzamlaKeres(szamlaszam);
                throw new ArgumentException("A megadott számlaszámmal" +
                    " már létezik számla", nameof(szamlaszam));
            }
            catch (HibasSzamlaszamException)
            {
                szamlak.Add(new Szamla(nev, szamlaszam));
            }
        }
        /// <summary>
        /// Lekérdezi az adott számlán lévő pénzösszeget
        /// </summary>
        /// <param name="szamlaszam">A számla számlaszáma, aminek az egyenlegét keressük</param>
        /// <returns>A számlán lévő egyenleg</returns>
        public ulong Egyenleg(string szamlaszam)
        {
            Szamla szamla = SzamlaKeres(szamlaszam);
            return szamla.Egyenleg;
        }

        /// <summary>
        /// Egy létező számlára pénzt helyez
        /// </summary>
        /// <param name="szamlaszam">A számla számlaszáma, amire pénzt helyez</param>
        /// <param name="osszeg">A számlára helyezendő pénzösszeg</param>
        /// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        public void EgyenlegFeltolt(string szamlaszam, ulong osszeg)
        {
            if (osszeg == 0)
            {
                throw new ArgumentException("Az összeg nem lehet 0",
                    nameof(osszeg));
            }
            Szamla szamla = SzamlaKeres(szamlaszam);
            szamla.Egyenleg += osszeg;
        }

        /// <summary>
        /// Két számla között utal.
        /// Ha nincs elég pénz a forrás számlán, akkor false értékkel tér vissza
        /// </summary>
        /// <param name="honnan">A forrás számla számlaszáma</param>
        /// <param name="hova">A cél számla számlaszáma</param>
        /// <param name="osszeg">Az átutalandó egyenleg</param>
        /// <returns>Az utalás sikeressége</returns>
        /// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        public bool Utal(string honnan, string hova, ulong osszeg)
        {
            if (osszeg == 0)
            {
                throw new ArgumentException("Az összeg nem lehet 0",
                    nameof(osszeg));
            }
            Szamla forras = SzamlaKeres(honnan);
            Szamla cel = SzamlaKeres(hova);
            bool sikeres = false;
            if (forras.Egyenleg >= osszeg)
            {
                sikeres = true;
                forras.Egyenleg -= osszeg;
                cel.Egyenleg += osszeg;
            }
            return sikeres;
        }

        /// <summary>
        /// Megkeresi a megadott számlaszámú számlát. A többi metódus ezt használja a számlák megkereséséhez
        /// </summary>
        /// <param name="szamlaszam">A keresendő számla számlaszáma</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">A számlaszám nem lehet üres</exception>
        /// <exception cref="ArgumentException">A számlaszám csak számot tartalmazhat</exception>
        /// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
        private Szamla SzamlaKeres(string szamlaszam)
        {
            if (szamlaszam == null)
            {
                throw new ArgumentNullException("A számlaszám nem lehet üres"
                    , nameof(szamlaszam));
            }
            if (String.IsNullOrEmpty(szamlaszam.Trim()))
            {
                throw new ArgumentException("A számlaszám nem lehet üres"
                    , nameof(szamlaszam));
            }
            if (Regex.IsMatch(szamlaszam, @"[^\d\s-]"))
            {
                throw new ArgumentException("A számlaszám csak számot tartalmazhat"
                    , nameof(szamlaszam));
            }

            int ind = 0;
            while (ind < szamlak.Count && szamlak[ind].Szamlaszam != szamlaszam)
            {
                ind++;
            }
            if (ind == szamlak.Count)
            {
                throw new HibasSzamlaszamException(szamlaszam);
            }
            return szamlak[ind];
        }
    }
}