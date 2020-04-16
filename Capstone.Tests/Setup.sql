-- Delete all of the data

ALTER TABLE campground
DROP CONSTRAINT FK__campgroun__park___2F10007B

ALTER TABLE site
DROP CONSTRAINT FK__site__campground__300424B4

DELETE FROM park
DELETE FROM campground;
DELETE FROM reservation;
DELETE FROM site;

-- Insert a fake park
INSERT INTO park
(name, location, establish_date, area, visitors, description)
VALUES
('Grand Canyon', 'Arizona', '01/20/1908', 1234, 19837, 'Beautiful Place'),
('Mount Rushmore', 'South Dakota', '01/20/1888', 1000, 13028, 'Big Presidents')
;

DECLARE @newParkId int = (SELECT @@IDENTITY);

-- Insert a fake campground
INSERT INTO campground
(name, open_from_mm, open_to_mm, daily_fee,park_id)
VALUES
('Rocky Road', 2, 10, 25.00, (SELECT park_id FROM park WHERE name = 'Grand Canyon')),
('Big Trees', 4, 8, 45.00, (SELECT park_id FROM park WHERE name = 'Mount Rushmore'));

DECLARE @newCampgroundId int = (SELECT @@IDENTITY);

-- Insert a fake site
INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
VALUES
((SELECT campground_id FROM campground WHERE name = 'Rocky Road'), 7, 8, 'true', 13, 'false'),
((SELECT campground_id FROM campground WHERE name = 'Big Trees'), 10, 4, 'false', 22, 'true');

DECLARE @newSiteId int = (SELECT @@IDENTITY);

--Insert a fake reservation
INSERT INTO reservation (site_id, name, from_date, to_date, create_date)
VALUES
((SELECT site_id FROM site WHERE campground_id = (SELECT campground_id FROM campground WHERE name = 'Rocky Road')), 'Top Mountain', '01/01/2019','05/19/2019', '12/25/2018'), 
((SELECT site_id FROM site WHERE campground_id = (SELECT campground_id FROM campground WHERE name = 'Big Trees')), 'Bottom Mountain', '05/05/2019','05/19/2019', '4/25/2018'); 


DECLARE @newReservationId int = (SELECT @@IDENTITY);



SELECT @newParkId as newParkId, @newCampgroundId as newCampgroundId, @newSiteId as newSiteId, @newReservationId as newReservationId;

