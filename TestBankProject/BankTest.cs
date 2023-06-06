using BankProject;

namespace TestBankProject
{
    public class BankTest
    {
        Bank b;

        [SetUp]
        public void Setup()
        {
            b = new Bank();
            b.UjSzamla("Gipsz Jakab", "1234");
        }

        [Test]
        public void UjSzamla_LetezoSzamlaszammal()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.UjSzamla("Teszt Elek", "1234");
            });
        }

        [Test]
        public void UjSzamla_LetezoNevvel()
        {
            Assert.DoesNotThrow(() =>
            {
                b.UjSzamla("Gipsz Jakab", "5678");
            });
        }

        [Test]
        public void UjSzamla_UresNevvel()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.UjSzamla("", "5678");
            });
        }

        [Test]
        public void UjSzamla_UresSzamlaszammal()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.UjSzamla("Gipsz Jakab", "");
            });
        }

        [Test]
        public void UjSzamla_NullNevvel()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                b.UjSzamla(null, "5678");
            });
        }

        [Test]
        public void UjSzamla_NullSzamlaszammal()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                b.UjSzamla("Gipsz Jakab", null);
            });
        }

        [Test]
        public void UjSzamla_SzamlaszamBetukkel()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.UjSzamla("Gipsz Jakab", "56A8");
            });
        }

        [Test]
        public void UjSzamla_NevSpecialisKarakter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.UjSzamla("Gipsz Jak!ab", "5678");
            });
        }

        [Test]
        public void UjSzamla_NevEkezettel()
        {
            Assert.DoesNotThrow(() =>
            {
                b.UjSzamla("Gipsz János", "5678");
            });
        }

        [Test]
        public void UjSzamla_SzamlaszamSzokozzel()
        {
            Assert.DoesNotThrow(() =>
            {
                b.UjSzamla("Gipsz Jakab", "5678 1234");
            });
        }

        [Test]
        public void UjSzamla_SzamlaszamKotojellel()
        {
            Assert.DoesNotThrow(() =>
            {
                b.UjSzamla("Gipsz Jakab", "5678-1234");
            });
        }

        [Test]
        public void Egyenleg_SzamlaszamBetuvel()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.Egyenleg("12ABC");
            });
        }

        [Test]
        public void Egyenleg_SzamlaszamUres()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.Egyenleg("");
            });
        }

        [Test]
        public void Egyenleg_SzamlaszamNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                b.Egyenleg(null);
            });
        }

        [Test]
        public void Egyenleg_NemLetezoSzamlaszam()
        {
            Assert.Throws<HibasSzamlaszamException>(() =>
            {
                b.Egyenleg("5678");
            });
        }

        [Test]
        public void Egyenleg_UjSzamlaEgyenlegeNulla()
        {
            Assert.AreEqual(0, b.Egyenleg("1234"));
        }

        [Test]
        public void EgyenlegFeltolt_NemLetezoSzamlaszam()
        {
            Assert.Throws<HibasSzamlaszamException>(() =>
            {
                b.EgyenlegFeltolt("5678", 10000);
            });
        }

        [Test]
        public void EgyenlegFeltolt_UresSzamlaszammal()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.EgyenlegFeltolt("", 10000);
            });
        }

        [Test]
        public void EgyenlegFeltolt_NullSzamlaszammal()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                b.EgyenlegFeltolt(null, 10000);
            });
        }

        [Test]
        public void EgyenlegFeltolt_NullaOsszeg()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                b.EgyenlegFeltolt("1234", 0);
            });
        }

        [Test]
        public void EgyenlegFeltolt_OsszegRakerulASzamlara()
        {
            b.EgyenlegFeltolt("1234", 10000);
            Assert.AreEqual(10000, b.Egyenleg("1234"));
        }

        [Test]
        public void EgyenlegFeltolt_OsszegHozzaadodikAJelenlegiEgyenleghez()
        {
            b.EgyenlegFeltolt("1234", 10000);
            Assert.AreEqual(10000, b.Egyenleg("1234"));
            b.EgyenlegFeltolt("1234", 20000);
            Assert.AreEqual(30000, b.Egyenleg("1234"));
        }
        [Test]
        public void EgyenlegFeltolt_MegfeleloSzamlaraTolt()
        {
            b.UjSzamla("Teszt Elek", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            b.EgyenlegFeltolt("5678", 50000);
            Assert.AreEqual(10000, b.Egyenleg("1234"));
            Assert.AreEqual(50000, b.Egyenleg("5678"));
        }
    }
}