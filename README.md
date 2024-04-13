Teknik Özellikler
    • Endpoint URL'leri, method'lar ve return type'lar RESTful standartlarına uygun olarak geliştirilmiştir. 
    • Onion katmanlı mimarisi ve Dependency Injection kullanılmıştır. Katmanlar arası veri transferi için AutoMapper kullanılmıştır. 
    • Her katmandaki method sonuçları için Generic ResponseDto<TErrorData, TSuccessData> sınıfı kullanılmıştır. 
    • Veritabanı olarak PostgreSQL kullanılmıştır. Tüm veritabanı işlemleri DB bağımsız olarak EF Core ve LINQ üzerinden gerçekleştirilmiştir. 
    • Validation ve action filter attribute kullanılarak, max/min uzunluk, doğru kategori/gün için sıra gibi gerekli kontroller yapılmıştır. Yanlış istekler için bad request dönülmüştür. 
    • Beklenmeyen hatalar middleware katmanında yakalanmış ve loglanmıştır. 
    • Authentication için cookie kullanılmıştır. Her cookie validasyonunda CustomCookieAuthenticationEvent kullanılarak, kullanıcı bilgileri kontrol edilir. Eğer kullanıcı bilgileri değişmişse, örneğin kullanıcı parolasını değiştirmişse veya hesabı kilitlenmişse, validasyon başarısız olur ve kullanıcı yeniden oturum açmalıdır. 
    • Authorization için .NET'in varsayılan PolicyProvider'ı override edilerek kullanıcıların roleId'lerine göre runtime'da policy'ler oluşturulmuştur. Böylece authorization attribute'ına verilecek herhangi bir roleId için tekrar policy tanımlamaya gerek kalmamıştır. 
    • Endpoint'ler uçtan uca async olarak geliştirilmiştir. 
    • Birlikte gerçekleşmesi gereken her işlem bir transaction içinde yapılmıştır. 
    • Kullanıcı şifreleri hashlenerek veritabanında tutulmuştur. Böylece veritabanı ele geçirilse bile şifreler güvende kalacaktır. 
    • Testler için solution içerisine Xunit projesi oluşturulmuştur. 


![image](https://github.com/cofidiss/foorestaurantapi-repo/assets/107804233/36b8f769-b900-45d1-beb1-117a663b7c64)
