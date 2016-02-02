var fs = require('fs');

module.exports = function(grunt) {

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    // Run compass
    // All the parameters below replace config.rb.
    compass: {
      options: {
        watch: false,
        cssDir: 'tmp',
        sassDir: 'scss',
        imagesDir: 'images',
        javascriptsDir: 'js',
        fontsDir: 'fonts',
        httpPath: '/',
        relativeAssets: true,
        noLineComments: true,
        importPath: [
          'node_modules/bootstrap/scss'
        ],
      },
      dist: {
        options: {
          environment: 'production',
          outputStyle: 'compressed',
        },
      },
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
        files: [ {expand: true, src: 'style.xsl', dest: '../dist'} ]
      }
    },

    'regex-replace': {
        foofoo: { //specify a target with any name
            src: ['../dist/style.xsl'],
            actions: [
                {
                    name: 'css',
                    search: new RegExp('(<link[^>]+>)'),
                     replace: function() {
                        return '<style>' + fs.readFileSync('tmp/style.css').toString() + '</style>';
                     },
                    flags: 'gi'
                }, {
                    name: 'js',
                    search: new RegExp('(<script.*\/script>)'),
                     replace: function() {
                        return '<script type="text/javascript" language="javascript"><![CDATA[' + fs.readFileSync('tmp/app.js').toString() + ']]></script>';
                     },
                    flags: 'gi'
                }
            ]
        }
    },

    exec: {
      xslt: 'xsltproc -o ../dist/report.html ../dist/style.xsl data.xml'
    },

    uglify: {
      my_target: {
        files: {
          'tmp/app.js': [
            'node_modules/jquery/dist/jquery.js',
            'node_modules/tether/dist/js/tether.min.js',
            'node_modules/bootstrap/dist/js/bootstrap.min.js',
            'node_modules/selectize/dist/js/standalone/selectize.min.js',
            'js/app.js',
          ]
        }
      }
    },

    // This is for dev only. Makes use of livereload on file changes.
    watch: {
      options: {
        debounceDelay: 1000,
      },
      js: {
        files: ['js/app.js'],
        tasks: ['build']
      },
      xslt: {
        files: ['style.xsl', 'data.xml'],
        tasks: ['exec']
      },
      scss: {
        files: ['scss/*.scss'],
        tasks: ['build']
      }
    }
  });

  // Define the modules we need for these tasks:
  grunt.loadNpmTasks('grunt-contrib-compass');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-regex-replace');
  grunt.loadNpmTasks('grunt-exec');

  // Here are our tasks 
  grunt.registerTask('default', ['build']);
  grunt.registerTask('build', ['copy', 'compass', 'concat', 'exec']);
  grunt.registerTask('dev', ['watch']);
  grunt.registerTask('boop', ['copy', 'compass', 'uglify', 'regex-replace', 'exec']);

};