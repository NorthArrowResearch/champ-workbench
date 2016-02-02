module.exports = function(grunt) {
  grunt.initConfig({

    // Run compass
    // All the parameters below replace config.rb.
    compass: {
      options: {
        watch: false,
        cssDir: 'dist',
        sassDir: 'src/scss',
        imagesDir: 'images',
        javascriptsDir: 'src/js',
        fontsDir: 'src/fonts',
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
            rename: function(dest, src){ return "src/scss/_selectize-bootstrap3.scss"; } }
        ],
      }
    },

    exec: {
      xslt: 'xsltproc -o dist/report.html style.xsl data.xml'
    },

    // Collect all our js into one script
    concat: {
      options: {
        separator: '\n;\n',
      },
      dist: {
        src: [
          'node_modules/jquery/dist/jquery.js',
          'node_modules/tether/dist/js/tether.min.js',
          'node_modules/bootstrap/dist/js/bootstrap.min.js',
          'node_modules/selectize/dist/js/standalone/selectize.min.js',
          'src/js/app.js',
        ],
        dest: 'dist/app.js',
      },
    },

    // This is for dev only. Makes use of livereload on file changes.
    watch: {
      options: {
        debounceDelay: 1000,
      },
      js: {
        files: ['src/js/app.js'],
        tasks: ['build']
      },
      xslt: {
        files: ['style.xsl', 'data.xml'],
        tasks: ['exec']
      },
      scss: {
        files: ['src/scss/*.scss'],
        tasks: ['build']
      }
    }
  });

  // Define the modules we need for these tasks:
  grunt.loadNpmTasks('grunt-contrib-compass');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-exec');

  // Here are our tasks 
  grunt.registerTask('default', ['build']);
  grunt.registerTask('build', ['copy', 'compass', 'concat', 'exec']);
  grunt.registerTask('dev', ['watch']);

};