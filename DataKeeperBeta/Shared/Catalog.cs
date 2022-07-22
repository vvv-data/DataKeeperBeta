
namespace Shared
{
    static class Catalog
    {       
        public const int count = 10;
        static public SubCatalog[] catalog = new SubCatalog[count]
        {
           new SubCatalog{Category = "E-mail", Heading = "E-mail адрес:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Социальные сети", Heading = "URL - социальной сети:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Интернет магазины", Heading = "URL - интернет магазина:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Онлайн сервисы", Heading = "URL - онлайн сервиса:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Онлайн банки", Heading = "URL - онлайн банка:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Электронные кошельки", Heading = "URL - электронного кошелька:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Кредитные карты", Heading = "Номер кредитной карты:", Description = "Данные кредитной карты:"},
           new SubCatalog{Category = "Коммунальные сервисы", Heading = "URL - коммунального сервиса:", Description = "Регистрационные данные:"},
           new SubCatalog{Category = "Приватные контакты", Heading = "ФИО - контакта:", Description = "Контактные данные:"},
           new SubCatalog{Category = "Разное", Heading = "Заголовок:", Description = "Информация:"}
        };              
    }
    class SubCatalog
    {
        public string Category { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
      
}
