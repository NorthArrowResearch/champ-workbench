# Workbench XML + XSL = HTML Reports

This is a proof of concept where we are taking an XML file + a XSL file and getting an HTML file. 

There is also a bit of CSS to make things pretty and a little JS to make it more functional.

## How to *consume* XSL reports

You don't need to do anything with node, grunt or sass if you just want to use this in .NET.

1. Use `style.xsl` to transform your xml file into an html file.
2. Place your new html file from the transform (`something.html`) next to `app.js` and `style.css` (found in the `/dist` folder of this repo).  
3. Feel pretty good about yourself and your connection with the universe. Go have some tea or something. It's your life, I don't know.

## How to *develop* XSL Reports

If you want to start editing XSL, CSS and JS you will need to get set up to compile these reports on the command line. This is assuming you already have `NodeJS` and `npm` (node package manager) installed and configured properly

1. Go to `cd src`
2. Run `npm install`. You should see libraries installing
3. Run `grunt`

If these two commands succeeded you are now good to go.

## Where do I edit X?

**FAST RULE: NEVER EDIT ANTYHING IN A `dist` OR `tmp` FOLDER. ALL FILES THERE WILL BE OVERWRITTEN. ALWAYS EDIT INSIDE `src`**

### XSL/HTML

To make changes to the output HTML you need to edit the XSL file responsible for generating it. 

* `src/REPORTNAME.xsl`: These files are the templates used to transform XML into HTML. 

### CSS

* `src/scss/_base.scss`: all CSS common to all reports
* `src/scss/REPORTNAME.scss`: CSS relevant to only one report

### JS

* `src/js/REPORTNAME.js`: Javascript relevant to a single report

To add a new library to a report first install it using `npm install -S <LIBNAME>`. The `-S` is important. Then edit the grunt file and look for the `concat` step. Add the path to your new library here and it will be compiled for you when you run grunt.

### Data

There is sample data included with the repo that is used to preview the changes you make and generate a sample report. 

* `Samples/REPORTNAME.xml`

## How do I make sure a change is applied

1. Edit the `src` file you want to change
2. run `grunt` to rebuild the `dist` xsl files
3. commit all changes (`src` and `dist`) to git.
4. Push changes.

## Grunt Workflow

When you run grunt the following steps get executed in order:

`['copy', 'compass', 'concat', 'regex-replace', 'exec']`

1. **Copy**: Copy source versions of the xsl to the `dist` folder
2. **Compass**: Compile all the `CSS` and `SCSS` into one file per report (look in the tmp folder)
3. **Concat**: Compile all the Javascript libraries into one file per report (look in the tmp folder)
4. **regex-replace**: This step smushes the CSS and JS into their respective XSL files so that when you compile an HTML file it is self-contained.
5. **exec**: This is a step for previewing the report only. It compiles a sample HTML file to preview your work

Running `grunt` on the command line will run all of these steps in order

Running `grunt dev` will run a watch process that will monitor all files for change and only run the appropriate steps of the process when it notices that something has changed.

## Running `xsltproc` manually

To generate your html file manually use the following command (assuming you have xsltproc installed)

```
xsltproc -o dist/report.html style.xsl data.xml
```