# Match 2 Oyunu Sistem Detayları ve Kişisel Notlar

## Sistem Detayları ve Açıklamalar

Sistemi açıklamadan önce bazı terimlerin ne ifade ettiklerini açıklayarak başlayayım;
- Tile Objesi: Oyundaki ana objeleri temsil eder (Bloklar, Roket, Balon vs.)
- Tile: Tile Objelerinin yuvasıdır ve onları yönetir.
- Grid: Tile'ları yaratan ve yöneten diktörgen şeklindeki alana verilen isim.

### Level Design Araçları - Level Editor Window (Unity Custom Editor Window)

Level Edtior Winow, bir level'ın temel yapıtaşı olan, `Grid Blueprint` Scriptable Object'ini düzenlemeye yardımcı olmak amacıyla yazılmış bir Custom Editor Window'dur. İçerdiği grid tabanlı görsellik sayesinde bir grid'in bütün tile'larını kolayca düzenleme imkanı sunar. Bu sayede level design process'ini daha verimli hale geitrir.

![Level Editor Window](/githubAssets/Images/ConfigMenu.png)

### Obje Üretme

Bu Match2 oyunu obje üretimi konusunda 2 yöntem entegre etmiştir;
- Dynamic Object Generation: Dinamik olarak obje gerektikçe yaratır ve işi bittiğinde yok eder.
- Object Pooling: Objelerin büyük bir çouğunluğunu level'ın başlamasıyla beraber yaratır. Gerektiğinde yarattığı objeleri aktifleştirir ve işi bittiğinde deaktive eder.

Obje üretme yöntemi, sahnelerdeki `TileObjectGenerator` GameObjesine istenilen üretim metodunun bir component olarak eklenmesi ile elde edilebilir. Pooling metodu 'Duck' Tile objesini üretme ve yok etme konusunda hatalar ile karşılaşmaktadır ve istenen şekilde davranmamaktadır. Onun haricindeki bütün Tile Objeleri ile düzgün çalışmaktadır.

### Blok Animasyonları

`TileMoveAnimation` Script'i bir Scriptable Object'tir ve Tile Objelerinin hareket ederken nasıl davranmaları gerektiğini tanımlayıp bu davranışı kaydetmeye yardımcı olur. `AnimationCurve` class'ını kullanarak jenerik animasyonlar üretmeyi sağlar. Assets/ScriptableObjects klasöründe 'TileToGoalAnimation' ve 'TileFallAnimaiton' dosyaları örnekler olarak gösterilebilir.

## Kişisel notlar
- Özellikle Tile Objelerini kodlarken OOP'den nasıl yararlanılması gerektiği konusundan ikilmelerim oldu. Örnek vermek gerekirse bütün Tile Objeleri `TileObject` sınıfından türüyorlar ve `TileObject` bazı abstract veya virtual
fonksiyonlara sahip olsa da bütün Tile Objeleri kapsayan bir şablon görevi görmüyor. Yani Tile Objeler kendi içlerinde sadece kendilerinin sahip olduğu davranışlar - fonksiyonlar tanımlayabiliyor. Bu da Tile Objelerini kullanırken type casting
işlemlerinin gerçekleştirilmesini gerektiriyor ve hataya daha açık (flag check yapılarak güvelik arttırıldı ve dynamic type check'ten kaçınıldı), okunması daha zor bir kod anlamına gelebiliyor. Diğer opsiyon ise `Tile` sınıfında kullandığım,
ana sınıfı abstract veya virtual fonksiyonlarla olabildiğince kapsayıcı hale getirmek ve polimorfizmle işi halletmek. Bundaki dezavantaj ise `TileAbsent` sınıfında görüldüğü gibi bazı fonskiyonlar için tanımı olmayan ama ana sınıfdan türediği için fonksiyonları implement etmesi gereken sınıfların, birçok
fonksiyonunu boş tanımlaması.
- Daha fazla zamanım olsaydı `LevelEditorWindow` Custom Editor'ını daha da geliştirmek isterdim. Çünkü bu oyun için en kritik noktanın bu editor olduğunu düşünüyorum. Level'ları kolayca tasarlayabilmek oyununun devamlı destkelenebilmesi için yüksek önem arz ediyor. İlk etapta geliştirmeler olarak düşündüklerim;
  - Editör sadece başlangıç grid'ini değil yukardan düşecek objelerin de tanımlanmasına müsade edebilmeli ve yukardan düşen objeler için; "Gerisini restgele üret", "5 tane kırmızı blok ekle" gibi araçlara sahip olmalı.
  - Birden fala tile seçilip hepsi tek tuşta atanabilmeli.
  - Tile Objelerinin resimleri render'lanmalı. 
