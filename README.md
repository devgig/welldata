# Well Data Viewer WPF Application
### Seeding Data
Select either WellData.csv or WellData.xlsx for [Import File] from the root directory.

### Data Interaction
Selecting a Well will load corresponding Tank data.

### Concepts Demonstrated
1.  Simple dirty data tracking on Tanks.  Updates are automatically executed when a record is left.  If a change is made and then reversed to the original, then the update will not be made.

2.  Use of Material Design for the themes.  The [Import File] button and GridSplitter are custom.

3.  Pattern for Busy Indicator progress bar and Loading.

4.  Use of AutoFac scanning for bootstrapping Caliburn Micro (View and ViewModel mapping), EF Core, a Strategy Pattern, and Implemented Interfaces except where scans are excluded.

5.  Using AutoFac for Testing bootstrap and sharing a common EF Core In-Memory database for all tests.  Utilizing Test Fixtures with EFCore.TestSupport to clean the database after each test is ran.  TestCaseOrder is used to run each EF Core database test sequentially.
---
![Screenshot](https://devgig.github.io/pages/welldata_screenshot.png)



