Online Alışveriş Platformu - ASP.NET Core Web API Projesi

Proje Yapısı
Bu proje, sorumlulukları ayırarak sürdürülebilirlik ve ölçeklenebilirliği artırmak amacıyla üç ana katmana ayrılmıştır.

1. Presentation Layer (API Katmanı)
Amacı: API uç noktalarını yönetir ve Business Layer ile etkileşim sağlar.
Bileşenler: HTTP isteklerini karşılayan controller sınıfları içerir.


3. Business Layer (İş Katmanı)
Amacı: İş mantığını kapsar ve Presentation Layer ile Data Access Layer arasında bir aracı olarak görev yapar.
Bileşenler: İş kurallarını ve hesaplamalarını içeren servis sınıfları içerir.


5. Data Access Layer (Veri Erişim Katmanı)
Amacı: Entity Framework Core kullanarak veritabanı işlemlerini yönetir.
Bileşenler: Repository (depo) ve Unit of Work (iş birimi) yapılarıyla veritabanı işlemlerini organize eder.

Kullanılan Teknolojiler
ASP.NET Core: Web API geliştirme
Entity Framework Core: Veritabanı yönetimi (Code-First yaklaşımı)
JWT (JSON Web Token): Kimlik doğrulama ve yetkilendirme
Data Protection: Kullanıcı şifrelerini güvenli bir şekilde saklama
Dependency Injection (DI): Servislerin bağımlılık yönetimi
MSSQL: Veritabanı yönetimi


Özellikler
1. Katmanlı Mimari
Proje, Presentation, Business ve Data Access olmak üzere üç ana katmana ayrılmıştır. Bu yapı, farklı görevleri yalıtarak uygulamanın yönetimini ve geliştirilmesini kolaylaştırır.

2. Veri Modelleri
Kullanıcı (User): Kullanıcı bilgilerini (id, ad, soyad, e-posta, rol, şifre) saklar.
Ürün (Product): Ürün bilgilerini (id, ad, fiyat, stok miktarı) içerir.
Sipariş (Order): Sipariş detaylarını (id, sipariş tarihi, toplam tutar, müşteri) saklar.
Sipariş Ürün (OrderProduct): Siparişler ve ürünler arasında çoka çok ilişki kurar.
3. Repository ve Unit of Work
Data Access Layer’da Repository deseni ile her bir model için ayrı veri erişim işlemleri yapılır. Unit of Work deseni ise bir işlemi birim olarak ele alarak veri bütünlüğünü sağlar.

4. Middleware
Logging Middleware: Tüm gelen istekleri loglar (URL, zaman ve kullanıcı bilgileri).
Maintenance Middleware: Uygulamanın bakım moduna alındığı durumlarda kullanıcıları bilgilendirir.


Kimlik Doğrulama ve Yetkilendirme
ASP.NET Core Identity veya Custom Identity Service ile kimlik doğrulama yapılır.
Kullanıcıların rol bazında (örneğin, "Admin" veya "Müşteri") yetkilendirilmesi sağlanır.
JWT kullanarak güvenli token tabanlı yetkilendirme sağlanır.

Middlewares
Logging Middleware: Gelen her isteğin URL’sini, zamanını ve kullanıcı kimliğini loglar.
Maintenance Middleware: Uygulamanın bakım moduna alınması durumunda kullanıcıya bakım mesajı ile yanıt verir.


Action Filter
Zaman Kısıtlamalı Erişim: Belirli API endpoint’lerine belirli zaman dilimlerinde erişim izni sağlar.


Model Doğrulama
Kullanıcı ve Ürün Modelleri için doğrulama kuralları uygulanmıştır:
Kullanıcı: E-posta formatı, benzersiz e-posta zorunluluğu, şifre minimum uzunluğu.
Ürün: Ürün adı zorunlu, fiyat ve stok miktarı pozitif olmalıdır.


Hata Yönetimi
Global Exception Handling: Tüm hataları yakalayarak kullanıcılara uygun bir hata mesajı döndürür.
