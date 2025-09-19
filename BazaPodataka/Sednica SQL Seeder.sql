-- Skupstina Srbije Baza Podataka -- 

DELETE FROM dbo.glasanje;
DELETE FROM dbo.pitanja;
DELETE FROM dbo.dnevni_red;
DELETE FROM dbo.sednica;
DELETE FROM dbo.zasedanje;
DELETE FROM dbo.mandat;
DELETE FROM dbo.lica;
DELETE FROM dbo.stranka;
DELETE FROM dbo.pozicija;
DELETE FROM dbo.tipovi;
DELETE FROM dbo.saziv;
GO

INSERT INTO dbo.saziv (id_saziva, ime, pocetak, kraj, opis) VALUES
(1, 'I Saziv Narodne skupštine', '1990-01-01', '1992-05-31', 'Prvi višestranački saziv izabran na izborima decembra 1990.'),
(2, 'II Saziv Narodne skupštine', '1992-06-01', '1993-12-31', 'Drugi saziv nakon vanrednih parlamentarnih izbora 1992.'),
(3, 'III Saziv Narodne skupštine', '1994-01-01', '1997-12-31', 'Treći saziv nakon vanrednih izbora 1993. zbog političke krize.'),
(4, 'IV Saziv Narodne skupštine', '1998-01-01', '2000-10-05', 'Četvrti saziv izabran na redovnim parlamentarnim izborima 1997.'),
(5, 'V Saziv Narodne skupštine', '2000-10-06', '2003-12-27', 'Peti saziv nakon vanrednih izbora posle pada režima Miloševića.'),
(6, 'VI Saziv Narodne skupštine', '2004-01-01', '2007-01-20', 'Šesti saziv nakon vanrednih izbora 2003. usled političke nestabilnosti.'),
(7, 'VII Saziv Narodne skupštine', '2007-01-21', '2008-05-10', 'Sedmi saziv izabran na redovnim parlamentarnim izborima 2007.'),
(8, 'VIII Saziv Narodne skupštine', '2008-05-11', '2012-05-05', 'Osmi saziv nakon vanrednih izbora 2008. zbog raspada koalicije.'),
(9, 'IX Saziv Narodne skupštine', '2012-05-06', '2014-03-15', 'Deveti saziv izabran na redovnim parlamentarnim izborima 2012.'),
(10, 'X Saziv Narodne skupštine', '2014-03-16', '2016-04-23', 'Deseti saziv nakon vanrednih izbora 2014. radi jačanja mandata.'),
(11, 'XI Saziv Narodne skupštine', '2016-04-24', '2020-06-20', 'Jedanaesti saziv nakon vanrednih izbora 2016. uprkos važećem mandatu.'),
(12, 'XII Saziv Narodne skupštine', '2020-06-21', '2022-04-02', 'Dvanaesti saziv nakon redovnih izbora 2020. Opozicija bojkotovala izbore.'),
(13, 'XIII Saziv Narodne skupštine', '2022-08-01', '2024-02-06', 'Trinaesti saziv nakon vanrednih parlamentarnih izbora 2022.'),
(14, 'XIV Saziv Narodne skupštine', '2024-02-07', '2028-02-07', 'Četrnaesti saziv formiran nakon redovnih izbora.');
GO

INSERT INTO dbo.tipovi (id, tip_zasedanja) VALUES
(1, 'Redovno zasedanje'),
(2, 'Vanredno zasedanje'),
(3, 'Svečano zasedanje'),
(4, 'Konstituirajuće zasedanje');
GO

INSERT INTO dbo.stranka (id_stranke, naziv_stranke) VALUES
(1, 'Srpska napredna stranka'),
(2, 'Socijalistička partija Srbije'),
(3, 'Demokratska stranka'),
(4, 'Dveri'),
(5, 'Socijaldemokratska partija Srbije'),
(6, 'Studentska Lista');
GO

INSERT INTO dbo.pozicija (id_pozicije, naziv_pozicije) VALUES
(1, 'Narodni poslanik'),
(2, 'Predsednik skupštine'),
(3, 'Potpredsednik skupštine'),
(4, 'Predsednik odbora'),
(5, 'Član odbora'),
(6, 'Lider poslaničke grupe');
GO

INSERT INTO dbo.lica (id_lica, ime, prezime, pozicija, stranka, pol, datumr, bio, korisnicko_ime,lozinka) VALUES
-- SNS 
(1, 'Aleksandar', 'Vučić', 2, 1, 'M', '1970-03-05', 'Predsednik Srbije, bivši predsednik vlade.', 'aleksandar.vučic', 'aleksandar123'),
(2, 'Ana', 'Brnabić', 1, 1, 'Ž', '1975-09-28', 'Bivša predsednica vlade.', 'ana.brnabic', 'ana123'),
(3, 'Maja', 'Gojković', 3, 1, 'Ž', '1963-05-22', 'Bivša predsednica skupštine.', 'maja.gojkovic', 'maja123'),
-- SPS 
(4, 'Ivica', 'Dačić', 1, 2, 'M', '1966-01-01', 'Ministar spoljnih poslova.', 'ivica.dacic', 'ivica123'),
(5, 'Zorana', 'Mihajlović', 1, 2, 'Ž', '1970-05-05', 'Bivša ministarka energetike.', 'zorana.mihajlovic', 'zorana123'),
-- DS 
(6, 'Dragan', 'Đilas', 6, 3, 'M', '1967-02-22', 'Bivši gradonačelnik Beograda.', 'dragan.dilas', 'dragan123'),
(7, 'Bojan', 'Pajtić', 1, 3, 'M', '1970-05-02', 'Bivši predsednik vlade Vojvodine.', 'bojan.pajtic', 'bojan123'),
-- Dveri 
(8, 'Boško', 'Obradović', 6, 4, 'M', '1976-01-03', 'Lider Dveri.', 'bosko.obradovic', 'bosko123'),
(9, 'Milan', 'Stamatović', 1, 4, 'M', '1972-11-11', 'Gradonačelnik Vrnjačke Banje.', 'milan.stamatovic', 'milan123'),
-- SDPS 
(10, 'Rasim', 'Ljajić', 6, 5, 'M', '1964-01-06', 'Bivši potpredsednik vlade.', 'rasim.ljajic', 'rasim123'),
(11, 'Nada', 'Laketić', 1, 5, 'Ž', '1963-08-15', 'Poslanik u više saziva.', 'nada.laketic', 'nada123'),
-- Studentska Lista 
(12, 'Marko', 'Milić', 1, 6, 'M', '1995-07-20', 'Predstavnik studenata sa Beogradskog univerziteta.', 'marko.milic', 'marko123'),
(13, 'Ana', 'Petrović', 1, 6, 'Ž', '1996-11-12', 'Predstavnica studenata sa Novosadskog univerziteta.', 'ana.petrović', 'ana123'),
(14, 'Nikola', 'Jovanović', 1, 6, 'M', '1997-03-30', 'Studentski aktivista.', 'nikola.jovanović', 'nikola123'),
-- Bivši
(15, 'Nebojša', 'Stefanović', 1, 1, 'M', '1976-11-20', 'Bivši ministar unutrašnjih poslova.', 'nebojsa.stefanovic', 'nebojsa123'),
(16, 'Goran', 'Knežević', 1, 1, 'M', '1962-09-09', 'Bivši ministar privrede.', 'goran.kneževic', 'goran123'),
(17, 'Slavica', 'Đukić-Dejanović', 1, 2, 'Ž', '1951-07-14', 'Bivša ministarka zdravlja.', 'slavica.dukic-dejanovic', 'slavica123'),
(18, 'Đorđe', 'Milićević', 1, 3, 'M', '1971-04-25', 'Opozicioni poslanik.', 'dorde.milićevic', 'dorde123'),
(19, 'Jelena', 'Zarić', 1, 5, 'Ž', '1980-12-05', 'Socijaldemokratska političarka.', 'jelena.zaric', 'jelena123'),
(20, 'Ivana', 'Smiljanić', 1, 6, 'Ž', '1998-02-18', 'Studentska predstavnica sa Niškog univerziteta.', 'ivana.smiljanic', 'ivana123');
GO

INSERT INTO dbo.mandat (id_mandata, id_lica, id_saziva, id_stranke) VALUES
--- SNS mandati
(1, 1, 14, 1),
(2, 2, 14, 1),
(3, 3, 14, 1),
(4, 15, 14, 1),
(5, 16, 14, 1),
--- SPS mandati
(6, 4, 14, 2),
(7, 5, 14, 2),
(8, 17, 14, 2),
--- DS mandati
(9, 6, 14, 3),
(10, 7, 14, 3),
(11, 18, 14, 3),
--- Dveri mandati
(12, 8, 14, 4),
(13, 9, 14, 4),
--- SDPS mandati
(14, 10, 14, 5),
(15, 11, 14, 5),
(16, 19, 14, 5),
--- Studentska lista mandati
(17, 12, 14, 6),
(18, 13, 14, 6),
(19, 14, 14, 6),
(20, 20, 14, 6);
GO

INSERT INTO dbo.zasedanje (id_zasedanja, tip, naziv_zasedanja, id_saziv) VALUES
(1, 1, 'Jesenje zasedanje XII saziva', 12),
(2, 1, 'Prolećno zasedanje XII saziva', 12),
(3, 1, 'Jesenje zasedanje XIII saziva', 13),
(4, 1, 'Prolećno zasedanje XIII saziva', 13),
(5, 1, 'Jesenje zasedanje XIV saziva', 14),
(6, 1, 'Prolećno zasedanje XIV saziva', 14);
GO

INSERT INTO dbo.sednica (id_sednice, naziv, datum, opis, zasedanje_id) VALUES
(1, 'Sednica 1 zasedanja 1', '2022-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 1),
(2, 'Sednica 2 zasedanja 1', '2022-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 1),
(3, 'Sednica 3 zasedanja 1', '2022-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 1),
(4, 'Sednica 4 zasedanja 1', '2022-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 1),
(5, 'Sednica 5 zasedanja 1', '2022-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 1),
(6, 'Sednica 1 zasedanja 2', '2023-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 2),
(7, 'Sednica 2 zasedanja 2', '2023-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 2),
(8, 'Sednica 3 zasedanja 2', '2023-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 2),
(9, 'Sednica 4 zasedanja 2', '2023-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 2),
(10, 'Sednica 5 zasedanja 2', '2023-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 2),
(11, 'Sednica 1 zasedanja 3', '2023-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 3),
(12, 'Sednica 2 zasedanja 3', '2023-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 3),
(13, 'Sednica 3 zasedanja 3', '2023-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 3),
(14, 'Sednica 4 zasedanja 3', '2023-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 3),
(15, 'Sednica 5 zasedanja 3', '2023-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 3),
(16, 'Sednica 1 zasedanja 4', '2024-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 4),
(17, 'Sednica 2 zasedanja 4', '2024-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 4),
(18, 'Sednica 3 zasedanja 4', '2024-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 4),
(19, 'Sednica 4 zasedanja 4', '2024-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 4),
(20, 'Sednica 5 zasedanja 4', '2024-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 4),
(21, 'Sednica 1 zasedanja 5', '2024-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 5),
(22, 'Sednica 2 zasedanja 5', '2024-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 5),
(23, 'Sednica 3 zasedanja 5', '2024-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 5),
(24, 'Sednica 4 zasedanja 5', '2024-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 5),
(25, 'Sednica 5 zasedanja 5', '2024-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 5),
(26, 'Sednica 1 zasedanja 6', '2025-03-10', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 1.', 6),
(27, 'Sednica 2 zasedanja 6', '2025-04-11', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 2.', 6),
(28, 'Sednica 3 zasedanja 6', '2025-05-12', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 3.', 6),
(29, 'Sednica 4 zasedanja 6', '2025-06-13', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 4.', 6),
(30, 'Sednica 5 zasedanja 6', '2025-07-14', 'Diskusija o zakonodavnim predlozima i tačkama dnevnog reda sednice 5.', 6);
GO

INSERT INTO dbo.dnevni_red (id_dnevni_red, id_sednice) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(11, 11),
(12, 12),
(13, 13),
(14, 14),
(15, 15),
(16, 16),
(17, 17),
(18, 18),
(19, 19),
(20, 20),
(21, 21),
(22, 22),
(23, 23),
(24, 24),
(25, 25),
(26, 26),
(27, 27),
(28, 28),
(29, 29),
(30, 30);
GO

INSERT INTO dbo.pitanja (id_pitanja, id_dnevni_red, redni_broj, tekst) VALUES
(1, 1, 1, 'Rasprava o tački 1 dnevnog reda sednice 1.'),
(2, 1, 2, 'Rasprava o tački 2 dnevnog reda sednice 1.'),
(3, 1, 3, 'Rasprava o tački 3 dnevnog reda sednice 1.'),
(4, 2, 1, 'Rasprava o tački 1 dnevnog reda sednice 2.'),
(5, 2, 2, 'Rasprava o tački 2 dnevnog reda sednice 2.'),
(6, 2, 3, 'Rasprava o tački 3 dnevnog reda sednice 2.'),
(7, 3, 1, 'Rasprava o tački 1 dnevnog reda sednice 3.'),
(8, 3, 2, 'Rasprava o tački 2 dnevnog reda sednice 3.'),
(9, 3, 3, 'Rasprava o tački 3 dnevnog reda sednice 3.'),
(10, 4, 1, 'Rasprava o tački 1 dnevnog reda sednice 4.'),
(11, 4, 2, 'Rasprava o tački 2 dnevnog reda sednice 4.'),
(12, 4, 3, 'Rasprava o tački 3 dnevnog reda sednice 4.'),
(13, 5, 1, 'Rasprava o tački 1 dnevnog reda sednice 5.'),
(14, 5, 2, 'Rasprava o tački 2 dnevnog reda sednice 5.'),
(15, 5, 3, 'Rasprava o tački 3 dnevnog reda sednice 5.'),
(16, 6, 1, 'Rasprava o tački 1 dnevnog reda sednice 6.'),
(17, 6, 2, 'Rasprava o tački 2 dnevnog reda sednice 6.'),
(18, 6, 3, 'Rasprava o tački 3 dnevnog reda sednice 6.'),
(19, 7, 1, 'Rasprava o tački 1 dnevnog reda sednice 7.'),
(20, 7, 2, 'Rasprava o tački 2 dnevnog reda sednice 7.'),
(21, 7, 3, 'Rasprava o tački 3 dnevnog reda sednice 7.'),
(22, 8, 1, 'Rasprava o tački 1 dnevnog reda sednice 8.'),
(23, 8, 2, 'Rasprava o tački 2 dnevnog reda sednice 8.'),
(24, 8, 3, 'Rasprava o tački 3 dnevnog reda sednice 8.'),
(25, 9, 1, 'Rasprava o tački 1 dnevnog reda sednice 9.'),
(26, 9, 2, 'Rasprava o tački 2 dnevnog reda sednice 9.'),
(27, 9, 3, 'Rasprava o tački 3 dnevnog reda sednice 9.'),
(28, 10, 1, 'Rasprava o tački 1 dnevnog reda sednice 10.'),
(29, 10, 2, 'Rasprava o tački 2 dnevnog reda sednice 10.'),
(30, 10, 3, 'Rasprava o tački 3 dnevnog reda sednice 10.'),
(31, 11, 1, 'Rasprava o tački 1 dnevnog reda sednice 11.'),
(32, 11, 2, 'Rasprava o tački 2 dnevnog reda sednice 11.'),
(33, 11, 3, 'Rasprava o tački 3 dnevnog reda sednice 11.'),
(34, 12, 1, 'Rasprava o tački 1 dnevnog reda sednice 12.'),
(35, 12, 2, 'Rasprava o tački 2 dnevnog reda sednice 12.'),
(36, 12, 3, 'Rasprava o tački 3 dnevnog reda sednice 12.'),
(37, 13, 1, 'Rasprava o tački 1 dnevnog reda sednice 13.'),
(38, 13, 2, 'Rasprava o tački 2 dnevnog reda sednice 13.'),
(39, 13, 3, 'Rasprava o tački 3 dnevnog reda sednice 13.'),
(40, 14, 1, 'Rasprava o tački 1 dnevnog reda sednice 14.'),
(41, 14, 2, 'Rasprava o tački 2 dnevnog reda sednice 14.'),
(42, 14, 3, 'Rasprava o tački 3 dnevnog reda sednice 14.'),
(43, 15, 1, 'Rasprava o tački 1 dnevnog reda sednice 15.'),
(44, 15, 2, 'Rasprava o tački 2 dnevnog reda sednice 15.'),
(45, 15, 3, 'Rasprava o tački 3 dnevnog reda sednice 15.'),
(46, 16, 1, 'Rasprava o tački 1 dnevnog reda sednice 16.'),
(47, 16, 2, 'Rasprava o tački 2 dnevnog reda sednice 16.'),
(48, 16, 3, 'Rasprava o tački 3 dnevnog reda sednice 16.'),
(49, 17, 1, 'Rasprava o tački 1 dnevnog reda sednice 17.'),
(50, 17, 2, 'Rasprava o tački 2 dnevnog reda sednice 17.'),
(51, 17, 3, 'Rasprava o tački 3 dnevnog reda sednice 17.'),
(52, 18, 1, 'Rasprava o tački 1 dnevnog reda sednice 18.'),
(53, 18, 2, 'Rasprava o tački 2 dnevnog reda sednice 18.'),
(54, 18, 3, 'Rasprava o tački 3 dnevnog reda sednice 18.'),
(55, 19, 1, 'Rasprava o tački 1 dnevnog reda sednice 19.'),
(56, 19, 2, 'Rasprava o tački 2 dnevnog reda sednice 19.'),
(57, 19, 3, 'Rasprava o tački 3 dnevnog reda sednice 19.'),
(58, 20, 1, 'Rasprava o tački 1 dnevnog reda sednice 20.'),
(59, 20, 2, 'Rasprava o tački 2 dnevnog reda sednice 20.'),
(60, 20, 3, 'Rasprava o tački 3 dnevnog reda sednice 20.'),
(61, 21, 1, 'Rasprava o tački 1 dnevnog reda sednice 21.'),
(62, 21, 2, 'Rasprava o tački 2 dnevnog reda sednice 21.'),
(63, 21, 3, 'Rasprava o tački 3 dnevnog reda sednice 21.'),
(64, 22, 1, 'Rasprava o tački 1 dnevnog reda sednice 22.'),
(65, 22, 2, 'Rasprava o tački 2 dnevnog reda sednice 22.'),
(66, 22, 3, 'Rasprava o tački 3 dnevnog reda sednice 22.'),
(67, 23, 1, 'Rasprava o tački 1 dnevnog reda sednice 23.'),
(68, 23, 2, 'Rasprava o tački 2 dnevnog reda sednice 23.'),
(69, 23, 3, 'Rasprava o tački 3 dnevnog reda sednice 23.'),
(70, 24, 1, 'Rasprava o tački 1 dnevnog reda sednice 24.'),
(71, 24, 2, 'Rasprava o tački 2 dnevnog reda sednice 24.'),
(72, 24, 3, 'Rasprava o tački 3 dnevnog reda sednice 24.'),
(73, 25, 1, 'Rasprava o tački 1 dnevnog reda sednice 25.'),
(74, 25, 2, 'Rasprava o tački 2 dnevnog reda sednice 25.'),
(75, 25, 3, 'Rasprava o tački 3 dnevnog reda sednice 25.'),
(76, 26, 1, 'Rasprava o tački 1 dnevnog reda sednice 26.'),
(77, 26, 2, 'Rasprava o tački 2 dnevnog reda sednice 26.'),
(78, 26, 3, 'Rasprava o tački 3 dnevnog reda sednice 26.'),
(79, 27, 1, 'Rasprava o tački 1 dnevnog reda sednice 27.'),
(80, 27, 2, 'Rasprava o tački 2 dnevnog reda sednice 27.'),
(81, 27, 3, 'Rasprava o tački 3 dnevnog reda sednice 27.'),
(82, 28, 1, 'Rasprava o tački 1 dnevnog reda sednice 28.'),
(83, 28, 2, 'Rasprava o tački 2 dnevnog reda sednice 28.'),
(84, 28, 3, 'Rasprava o tački 3 dnevnog reda sednice 28.'),
(85, 29, 1, 'Rasprava o tački 1 dnevnog reda sednice 29.'),
(86, 29, 2, 'Rasprava o tački 2 dnevnog reda sednice 29.'),
(87, 29, 3, 'Rasprava o tački 3 dnevnog reda sednice 29.'),
(88, 30, 1, 'Rasprava o tački 1 dnevnog reda sednice 30.'),
(89, 30, 2, 'Rasprava o tački 2 dnevnog reda sednice 30.'),
(90, 30, 3, 'Rasprava o tački 3 dnevnog reda sednice 30.');
GO

INSERT INTO dbo.glasanje (id_glasanja, id_pitanja, id_lica, glas) VALUES
--- Pitanje 76
(1, 76, 1, 'ZA'),
(2, 76, 2, 'ZA'),
(3, 76, 3, 'ZA'),
(4, 76, 4, 'ZA'),
(5, 76, 5, 'ZA'),
(6, 76, 15, 'ZA'),
(7, 76, 16, 'ZA'),
(8, 76, 17, 'ZA'),
(9, 76, 10, 'ZA'),
(10, 76, 11, 'ZA'),
(11, 76, 19, 'ZA'),
(12, 76, 6, 'PROTIV'),
(13, 76, 7, 'PROTIV'),
(14, 76, 8, 'PROTIV'),
(15, 76, 9, 'PROTIV'),
(16, 76, 12, 'PROTIV'),
(17, 76, 13, 'PROTIV'),
(18, 76, 14, 'PROTIV'),
(19, 76, 18, 'PROTIV'),
(20, 76, 20, 'PROTIV'),

--- Pitanje 77 
(21, 77, 1, 'ZA'),
(22, 77, 2, 'ZA'),
(23, 77, 3, 'ZA'),
(24, 77, 4, 'ZA'),
(25, 77, 5, 'ZA'),
(26, 77, 15, 'ZA'),
(27, 77, 16, 'ZA'),
(28, 77, 17, 'SUZDRŽAN'),
(29, 77, 10, 'ZA'),
(30, 77, 11, 'SUZDRŽAN'),
(31, 77, 19, 'ZA'),
(32, 77, 6, 'PROTIV'),
(33, 77, 7, 'PROTIV'),
(34, 77, 8, 'PROTIV'),
(35, 77, 9, 'PROTIV'),
(36, 77, 12, 'PROTIV'),
(37, 77, 13, 'SUZDRŽAN'),
(38, 77, 14, 'PROTIV'),
(39, 77, 18, 'PROTIV'),
(40, 77, 20, 'SUZDRŽAN'),

--- Pitanje 78 
(41, 78, 1, 'ZA'),
(42, 78, 2, 'ZA'),
(43, 78, 3, 'ZA'),
(44, 78, 4, 'ZA'),
(45, 78, 5, 'ZA'),
(46, 78, 15, 'ZA'),
(47, 78, 16, 'ZA'),
(48, 78, 17, 'ZA'),
(49, 78, 10, 'ZA'),
(50, 78, 11, 'ZA'),
(51, 78, 19, 'ZA'),
(52, 78, 6, 'ZA'),
(53, 78, 7, 'ZA'),
(54, 78, 8, 'ZA'),
(55, 78, 9, 'ZA'),
(56, 78, 12, 'ZA'),
(57, 78, 13, 'ZA'),
(58, 78, 14, 'ZA'),
(59, 78, 18, 'ZA'),
(60, 78, 20, 'ZA');
GO

PRINT 'Uspesno upisani podaci u bazu podataka Skupstine Srbije.';