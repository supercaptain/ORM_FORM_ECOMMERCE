
INSERT INTO 
CATEGORY (name, description)
VALUES
('Mac', 'Poèítaèe od Apple. Macbooky a iMac nebo Mac Mini'),
('iPad', 'Tablety od Apple.'),
('iPhone', 'Telefony od Apple.'),
('Watch', 'Hodinky od Apple.'),
('iPod', 'Hudebni prehávace od Apple.'),
('Prislusenstvi', 'Prislusenstvi pro Apple produkty.')


INSERT INTO 
TYPEDelivery (name, cost)
VALUES
('Ceska Posta', 100),
('DHL', 120),
('PPL', 110),
('Geis', 130)



INSERT INTO
PRODUCT (idCategory, name, description, currentCost)
VALUES
(1, 'iMac 27" ', 'Stolni pocitac od Apple s velkym rozlisenim', 59000),
(1, 'iMac 21" ', 'Stolni pocitac od Apple s velkym rozlisenim', 39000),
(1, 'Macbook Air 11" ', 'Prenosny pocitac od Apple ', 25000),
(1, 'Macbook Air 13" ', 'Prenosny pocitac od Apple ', 32000),
(1, 'Macbook Pro 13" ', 'Prenosny pocitac od Apple ', 39000),
(1, 'Macbook Pro 15" ', 'Prenosny pocitac od Apple ', 61000),
(2, 'iPad Mini s Retina Displayem" ', 'Table od Apple ipad Mini', 9000),
(2, 'iPad Air" ', 'table od Apple ipad Air', 10000),
(2, 'iPad Air 2" ', 'table od Apple ipad Air', 12000),
(3, 'iPhone 5 ', 'iphone od apple ', 10000),
(3, 'iPhone 5S ', 'iphone od apple ', 15000),
(3, 'iPhone 6 ', 'iphone od apple ', 19080),
(3, 'iPhone 6+ ', 'iphone od apple ', 22500),
(4, 'Watch Sport ', 'Nejlevnejsi hodinky od Apple', 11599),
(4, 'Watch ', 'Stredni edice hodinek od Apple', 35500),
(4, 'Watch Edition ', 'Nejdrazsi hodinky od Apple', 255000),
(5, 'iPod Touch ', 'Prehavac od Apple', 5600),
(5, 'iPad Nano ', 'Prehavac od Apple', 2600),
(6, 'Taska na Macbook Air ', 'Taska na Macbook Air s prodysneho materialu', 1000),
(6, 'Taska na Macbook Pro ', 'Taska na Macbook Air s prodysneho materialu', 1200),
(6, 'Obal na Macbook iPad ', 'Obal na iPad s prodysneho materialu', 1400)





INSERT INTO
Customer (fname, lname, type,phone)
values
('Adam', 'Konecny', 'admin', '+420608112268')

INSERT INTO
Customer (fname, lname,phone)
values
('Petr', 'Konecny', '+420608101010'),
('Filip', 'Konecny', '+420608101010'),
('Petr', 'Chovny',  '+420608112268'),
('Nicholas', 'Morgan','+420608112268'),
('Cheryl', 'Walker','+420608112268'),
('Lois', 'Lawrence', '+420608112268'),
('Stephanie', 'Armstrong', '+4206123456'),
('Clarence', 'Simmons', '+420608112268')



INSERT INTO 
Login(idCustomer, email, password)
VALUES

(1, 'adam@email.cz', '123456'),
(2, 'petr@email.cz', '123456789'),
(3, 'filip@email.cz', '123456789'),
(4, 'petrchovny@email.cz', '123456789'),
(5, 'nicholas@email.cz', '123456789'),
(6, 'cherylp@email.cz', '123456789'),
(7, 'lois@email.cz', '123456789'),
(8, 'stephanie@email.cz', '123456789'),
(9, 'clarence@email.cz', '123456789')






INSERT INTO 
[order]
(idCustomer, iddelivery, orderdate, cost, status, adress,  city, postalcode)
VALUES
(1, 1, '01.10.2015', 59000+100, 'prijata' , 'Maroldova 3', 'Ostrava', '70200'),
(2, 1, '01.11.2015', 59000+100 , 'odeslana', 'Maroldova 3', 'Ostrava', '70200'),
(3, 1, '01.12.2015', 59000+100, 'zaplacena', 'Rudna 3', 'Ostrava', '70800'),
(4, 2, '01.12.2015', 9000+120, 'zaplacena' , 'Namesti Svobody', 'Praha', '20890'),
(4, 2, '03.20.2015', 11599+110, 'zaplacena','Namesti Svobody', 'Praha', '20890'),
(1, 1, '03.26.2015', 22500+100, 'odeslana', 'Maroldova 32', 'Ostrava', '70200'),
(3, 3, '04.02.2015', 25000+100, 'odeslana', 'Rudna 15', 'Ostrava', '70800')



INSERT INTO 
OrderDetail
(idOrder, idProduct, quantity)
VALUES
(1,1,1),
(2,1,1),
(3,1,1),
(4,7,1),
(5,14,1),
(6,13,1),
(7,3,1)


INSERT INTO 
ProductCostHistory (idProduct, stardate, enddate, cost)
VALUES
(1, '02.04.2014', '06.10.2014', 63000),
(1, '07.04.2014', '12.10.2014', 61000)


INSERT INTO
WatchDog (idCustomer,idProduct, createDate, cost, notify)
VALUES
(1,9,'04.04.2015',12000,0),
(1,20,'04.04.2015',1200,0)




