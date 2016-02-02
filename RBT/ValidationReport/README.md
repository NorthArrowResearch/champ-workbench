# RBT Report XML + XSL = HTML

This is a proof of concept where we are taking an XML file + a XSL file and getting an HTML file. 

There is also a bit of CSS to make things pretty and a little JS to make it more functional.

## How to consume this repo

You don't need to do anything with node, grunt or sass if you just want to use this.

1. Use `style.xsl` to transform your xml file into an html file.
2. Place your new html file from the transform (`something.html`) next to `app.js` and `style.css` (found in the `/dist` folder of this repo).  
3. Feel pretty good about yourself and your connection with the universe. Go have some tea or something. It's your life, I don't know.


## Running `xsltproc` manually

To generate your html file manually use the following command (assuming you have xsltproc installed)

```
xsltproc -o dist/report.html style.xsl data.xml
```

## The Grunt way

### Setting up: 

```
npm install
```

### Running:

```
grunt dev
```

Then make changes. Grunt will watch any files for changes and rebuild everything.