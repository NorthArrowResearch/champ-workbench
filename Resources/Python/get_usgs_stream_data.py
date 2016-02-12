import urllib2, StringIO, gzip, xml, os, sys, argparse
from xml.dom.minidom import parse

##TODO
#fix exception handling
#create error handling for HTTP error codes from USGS

parser = argparse.ArgumentParser()
parser.add_argument('SiteNumber',
                    help = 'Site number of USGS gage to get stream data for.',
                    type = str)

parser.add_argument('OutputPath',
                    help = 'Output directory to store data in.',
                    type = str)

parser.add_argument('OverwriteFile',
                    help = 'Boolean value to indicate if the downloaded data exists in Output Directory should it be overwritten.',
                    type = str)

args = parser.parse_args()

##site_number = 14044000
##output_directory = r'C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS'
boolean_overwrite = True

##huc
#url = r'http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&huc=17070201&parameterCd=00060&siteType=ST&startDT=2011-01-01&endDT=2016-01-01'
#url = r'http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&huc=17070201,17070202,17070203,17070204&parameterCd=00060&siteType=ST&startDT=2011-01-01&endDT=2016-01-01'


def create_usgs_stream_gage_api_request(site_number):
    #create initial request of compressed data
    url = r'http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&site={0}&parameterCd=00060&siteType=ST&startDT=2011-01-01&endDT=2016-01-01'.format(site_number)
    request = urllib2.Request(url)
    request.add_header('Accept-Encoding', 'gzip,compress')
    opener = urllib2.build_opener()
    #print 'Request initialized at: {0}'.format(url)
    request_data = opener.open(request)
    #print 'Request successful.'
    return request_data

def decompress_request_data(request_data):
    #decompress the data
    #print 'Reading compressed data............'
    compressed_data = request_data.read()
    compressed_stream = StringIO.StringIO(compressed_data)
    g_zipper = gzip.GzipFile(fileobj=compressed_stream)
    #print 'Uncompressing data.................'
    decompressed_data = g_zipper.read()
    return decompressed_data

def check_output_path(output_path, site_number, boolean_overwrite):
    output_directory = os.path.dirname(output_path)
    if os.path.isdir(output_directory):
        #output_path = os.path.join(output_directory, 'site_{0}_usgs_discharge.xml'.format(site_number))
        if os.path.isfile(output_path):
            if boolean_overwrite == True:
                os.remove(output_path)
                return output_path
            elif boolean_overwrite == False:
                raise IOError
        elif os.path.isfile(output_path) == False:
            return output_path
    elif os.path.isdir(output_directory) == False:
        raise IOError

def write_pretty_xml(decompressed_data, output_directory, site_number, boolean_overwrite):
    #create and check output path
    output_path = check_output_path(output_directory, site_number, boolean_overwrite)

    #parse xml and write
    #print 'Parsing to pretty xml and writing to file...............'
    xml_string = xml.dom.minidom.parseString(decompressed_data)
    with open(output_path,'w') as w_file:
        w_file.write(xml_string.toprettyxml())

    #print 'File successfully written at: {0}'.format(output_path)
    return output_path

def get_usgs_stream_data(site_number, out_directory, boolean_overwrite):
    try:
        request_data = create_usgs_stream_gage_api_request(site_number)
        decompressed_data = decompress_request_data(request_data)
        output_path = write_pretty_xml(decompressed_data, out_directory, site_number, boolean_overwrite)
        if os.path.isfile(output_path):
            return output_path
        else:
            raise IOError    
    except IOError as e:
        print 'I/O error [{0}]: {1}\n Bad path: {2}'.format(e.errno, e.strerror, e.filename)
    except:
        print 'Unxexpected error: {0}'.format(sys.exc_info()[0])
        raise

if args.OverwriteFile == 1:
    boolean_overwrite = True
elif args.OverwriteFile == 0:
    boolean_overwrite = False

output_path = get_usgs_stream_data(args.SiteNumber, args.OutputPath, boolean_overwrite)
print output_path

