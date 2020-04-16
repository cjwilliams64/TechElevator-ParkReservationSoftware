Module 2 Capstone - National Park Campsite Reservation

Congratulations! You did such a great job on your previous application we want you to build our new campsite
reservation application. We are tasking you to build a command line driven application that our National Park
Service can use to book campsite reservations.

The requirements for your application are listed below:
1. As a user of the system, I need the ability to view a list of the available parks in the system, sorted
alphabetically by name.
a. A park includes an id, name, location, established date, area, annual visitor count, and
description.
2. As a user of the system, I need the ability to select a park that my customer is visiting and see a list of
all campgrounds for that available park.
a. A campground includes an id, name, open month, closing month, and a daily fee.
3. As a user of the system, I need the ability to select a campground and search for date availability so
that I can make a reservation.
a. A reservation search only requires the desired campground, a start date, and an end date.
b. A campsite is unavailable if any part of their preferred date range overlaps with an existing
reservation.
c. If no campsites are available, indicate to the user that there are no available sites and ask them
if they would like to enter in an alternate date range.

​

d. The TOP 5 available campsites should be displayed along with the cost for the total stay.
e. BONUS: If a date range is entered that occurs during the park off-season, then the user should
not see any campsites available for reservation.
f.

BONUS: Provide an advanced search functionality allowing users to indicate any requirements
they have for maximum occupancy, requires wheelchair accessible site, an rv and its length if
required, and if a utility hookup is necessary.

4. As a user of the system, once I find a campsite that is open during the time window I am looking for, I
need the ability to book a reservation at a selected campsite.
a. A reservation requires a name to reserve under, a start date, and an end date.
b. A confirmation id is presented to the user once the reservation has been submitted.
5. BONUS: As a user of the system, I want the ability to select a park and search for campsite availability
across the entire park so that I can make a reservation.
a. Up to 5 campsites for each campground (if applicable) should be displayed if they have
availability along with the cost of the total stay.
b. The same rules apply as the campground search.
6. BONUS: As a user of the system, I would like the ability to see a list of all upcoming reservations within
the next 30 days for a selected national park.

Sample Screens
View Parks Interface
Select a Park for Further Details
1) Acadia
2) Arches
3) Cuyahoga National Valley Park
4) …
Q) quit

Park Information Screen
Acadia National Park
Location:
Maine
Established:
02/26/1919
Area:
47,389 sq km
Annual Visitors: 2,563,129
Covering most of Mount Desert Island and other coastal islands, Acadia features the
tallest mountain on the Atlantic coast of the United States, granite peaks, ocean
shoreline, woodlands, and lakes. There are freshwater, estuary, forest, and intertidal
habitats.
Select a Command
1) View Campgrounds
2) Search for Reservation
3) Return to Previous Screen

Park Campgrounds
Acadia National Park Campgrounds

#1
#2
#3

Name
Blackwoods
Schoodic Woods
Seawall

Open
January
May
May

Close
December
October
September

Select a Command
1) Search for Available Reservation
2) Return to Previous Screen

Daily Fee
$35.00
$30.00
$30.00

Search for Campground Reservation
Name
Open
#1
Blackwoods
January
#2
Schoodic Woods
May
#3
Seawall
May

Close
December
October
September

Daily Fee
$35.00
$30.00
$30.00

Which campground (enter 0 to cancel)? __
What is the arrival date? __/__/____
What is the departure date? __/__/____
Results Matching Your Search Criteria
Site No.
Max Occup. Accessible?
1
4
No
4
6
Yes
13
12
Yes

Max RV Length
N/A
N/A
20

Utility
N/A
N/A
Yes

Cost
$XX
$XX
$XX

Which site should be reserved (enter 0 to cancel)? __
What name should the reservation be made under? __
The reservation has been made and the confirmation id is {Reservation_id}
BONUS: Search for Park-wide Reservation
What is the arrival date? __/__/____
What is the departure date? __/__/____
Results Matching Your Search Criteria
Campground Site No.
Max Occup. Accessible?
Blackwoods 1
4
No
Seawall
4
6
Yes
Seawall
13
12
Yes

RV Len
N/A
N/A
20

Utility
N/A
N/A
Yes

Which site should be reserved (enter 0 to cancel)? __
What name should the reservation be made under? __
The reservation has been made and the confirmation id is {Reservation_id}

Cost
$XX
$XX
$XX

Data Sources
Your application will have access to a Relational Database populated with data.

Park Table
A parks table is provided to the system that provides the data for each of the supported national parks. The
data columns are as follows:

PK

Field

Description

park_id

A surrogate key for the park.

name

The name of the park

location

The location of the park

establish_date

The date that the park was established

area

The size of the park in square kilometers

visitors

The annual number of visitors to the park

description

A short description about the park

Campground Table
A campground table is provided to the system that provides a list of the one or many campgrounds located
inside of a national park. The data columns are as follows:
Field

Description

PK

campground_id

A surrogate key for the campground.

FK

park_id

The park that the campground is associated with.

name

The name of the campground.

open_from_mm

The numerical month the campground is open for reservation.
( 01 - January, 02 - February, …)

open_to_mm

The numerical month the campground is closed for reservation.
( 01 - January, 02 - February, …)

daily_fee

The daily fee for booking a campsite at this campground

Site Table
A site table lists all of the available campsites available for reservation in a campground.The data columns are
as follows:
Field

Description

PK

site_id

A surrogate key for the campsite.

FK

campground_id

The campground that the park belongs to.

site_number

The arbitrary campsite number

max_occupancy

Maximum occupancy at the campsite

accessible

Indicates whether or not the campsite is handicap accessible

max_rv_length

The maximum rv length that the campsite can fit. 0 indicates that
no RV will fit at this campsite.

utilities

Indicates whether or not the campsite provides access to utility
hookup.

Reservation
The reservation table lists all of the past, current, and future reservations for a campsite in the national park
system. The data columns are as follows:
Field

Description

PK

reservation_id

A surrogate key for the reservation.

FK

site_id

The campsite the reservation is for.

name

The name for the reservation.

from_date

The start date of the reservation.

to_date

The end date of the reservation.

create_date

The date the reservation was booked.

