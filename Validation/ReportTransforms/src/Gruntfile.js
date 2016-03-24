var fs = require('fs');

module.exports = function(grunt) {

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    // Grunt-sass 
    // sass (libsass) config
    sass: {
        options: {
            sourceMap: true,
            relativeAssets: false,
            outputStyle: 'compressed',
            sassDir: 'scss',
            cssDir: 'tmp',
            includePaths: ['node_modules/bootstrap/scss']
        },
        build: {
            files: [{
                expand: true,
                cwd: 'scss/',
                src: ['**/*.scss'],
                dest: 'tmp',
                ext: '.css'
            }]
        }
    },

    // Move our assets out of node_modules
    copy: {
      themes: {
        files: [
          { expand:true, cwd:'node_modules/selectize/dist/css', src:'selectize.bootstrap3.css', 
            rename: function(dest, src){ return 'scss/_selectize-bootstrap3.scss'; } }
        ],
      },
      xsl: {
        files: [ {expand: true, src: '*.xsl', dest: '../dist'} ]
      }
    },

    // convert the file name into a <script> or <style> tag containing that file
    'regex-replace': {
      xslfiles: { //specify a target with any name
        src: ['../dist/*.xsl'],
        actions: [
          {
            name: 'css',
            search: /<link href="([^"]+.css)[^>]+>/i,
             // m0 is the whole match. m1 is the match inside the ()
             replace: function(m0, m1) {
                
                return '<style>' + fs.readFileSync(m1).toString() + '</style>';
             },
            flags: 'gi'
          }, {
            name: 'js',
            search: /<script src="([^"]+.js)[^>]+><\/script>/i,
             // m0 is the whole match. m1 is the match inside the ()
             replace: function(m0,m1) {
                return '<script type="text/javascript" language="javascript"><![CDATA[' + fs.readFileSync(m1).toString() + ']]></script>';
             },
            flags: 'gi'
          }
        ]
      }
    },

    exec: {
      gcd:        'xsltproc -o ../Samples/gcd.html ../dist/gcd.xsl ../Samples/gcd.xml',
      habitat:    'xsltproc -o ../Samples/habitat.html ../dist/habitat.xsl ../Samples/habitat.xml',
      rbt:        'xsltproc -o ../Samples/rbt.html ../dist/rbt.xsl ../Samples/rbt.xml',
      rbt_manual: 'xsltproc -o ../Samples/rbt_manual.html ../dist/rbt_manual.xsl ../Samples/rbt_manual.xml'
    },

    // Collect all our js into one script
    concat: {
      options: {
        separator: '\n;\n',
      },
      rbt_manual: {
        src: [
          'node_modules/jquery/dist/jquery.min.js',
          'node_modules/tether/dist/js/tether.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/selectize/dist/js/standalone/selectize.min.js',
          'js/rbt_manual.js',
        ],
        dest: 'tmp/rbt_manual.js'      
      },
      gcd: {
        src: [
          'node_modules/jquery/dist/jquery.min.js',
          'node_modules/tether/dist/js/tether.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/d3/d3.min.js',
          'js/gcd.js',
        ],
        dest: 'tmp/gcd.js' 
      },
      habitat: {
        src: [
          'node_modules/jquery/dist/jquery.min.js',
          'node_modules/tether/dist/js/tether.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/d3/d3.min.js',
          'js/habitat.js',
        ],
        dest: 'tmp/habitat.js'    
      },
      rbt: {
        src: [
          'node_modules/jquery/dist/jquery.min.js',
          'node_modules/tether/dist/js/tether.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/d3/d3.min.js',
          'js/lib/boxplot.js',
          'js/rbt.js',
        ],
        dest: 'tmp/rbt.js'    
      }
    },

    // This is for dev only. Makes use of livereload on file changes.
    watch: {
      options: {
        debounceDelay: 1000,
      },
      js: {
        files: ['js/*.js'],
        tasks: ['copy', 'concat', 'regex-replace', 'exec']
      },
      xslt: {
        files: ['*.xsl', '../Samples/*.xml'],
        tasks: ['copy', 'regex-replace', 'exec']
      },
      scss: {
        files: ['scss/*.scss'],
        tasks: ['build']
      }
    }
  });

  // Define the modules we need for these tasks:
  grunt.loadNpmTasks('grunt-sass');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-regex-replace');
  grunt.loadNpmTasks('grunt-exec');

  // Here are our tasks 
  grunt.registerTask('default', ['build']);
  grunt.registerTask('build', ['copy', 'sass', 'concat', 'regex-replace']);
  grunt.registerTask('buildsamples', ['build', 'exec']);
  grunt.registerTask('dev', ['watch']);

};