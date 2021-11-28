import rhinoscriptsyntax as rs
import os
from os.path import basename
import Rhino

path = dir_path = os.path.dirname(os.path.realpath('Benchmark_runner.py'))
list_folder = list(os.walk(path))
num_tests = len(list_folder[0][1])
benchmarks = range(0,num_tests)

pass_tests = []
tests_passed = True

summary = open('summary.txt', 'w')
num_failed=0

for i in benchmarks:
    test_tmp = (True,'')
    pass_tests.append(test_tmp)
    same = True
    #load paths for input and output
    #path = dir_path = os.path.dirname(os.path.realpath('Benchmark_runner.py'))
    inpath = path + "\\" + list_folder[0][1][i]
    for root, dirs, files in os.walk(inpath):
        for file in files:
            if file.endswith('.3dm'):
                file_name = os.path.splitext(file)[0]
    in_filename =  file_name + ".3dm"
    outpath = path + "\\" + list_folder[0][1][i]

    Rhino.RhinoApp.RunScript("_-Open \n \"" + inpath + "\\" + in_filename + "\"\n ",False)
    Rhino.RhinoApp.RunScript("_-TeDA_WriteInput \n Benchmark \"" + outpath + "\"\n ",False)

    #initialize file for differences
    FO = open(outpath + "\\differences.txt", 'w')

    files = {"geometry.cad.json", "materials.json", "physics.iga.json", "ProjectParameters.json", "refinements.iga.json"}

    #check files
    for file in files:
        test_file = open(outpath + "\\" + file, "r")
        reference_filename =  file_name + "_" + file
        reference_file = open(outpath + "\\" + reference_filename, "r")
        lines_test=test_file.readlines()
        lines_reference=reference_file.readlines()

        #compare textfiles
        if (len(lines_test) == len(lines_reference)):
            for li in range(0,len(lines_test)):
                if (lines_test[li] != lines_reference[li]):
                    FO.write(file)
                    FO.write(": line %i:\n %s %s\n" %(li+1, lines_test[li], lines_reference[li]))
                    same = False
                    #break
        else:
            same = False
            FO.write("Geometrry files do not have the same length!\n")

        test_file.close()
        os.remove(outpath + "\\" + file)
        reference_file.close()

    os.remove(outpath + "\\" + "Benchmark_kratos_0.georhino.json")
    os.remove(outpath + "\\" + "kratos_main_iga.py")

    FO.close()

    #set final flags for testing
    if (same == False):
        pass_tests[i]=(False,file_name)
        num_failed=num_failed+1
        tests_passed = False

#write summary
if (tests_passed):
    summary.write("All tests were passed!\n")
    Rhino.RhinoApp.WriteLine("All tests passed!")
else:
    #num_failed=0
    failed = []
    for i in range(0,num_tests):
        if (pass_tests[i][0]==False):
            #num_failed += 1
            failed.append(pass_tests[i][1])
    summary.write("%i of %i tests passed.\n" %((num_tests - num_failed),num_tests))
    summary.write("The following Benchmarks failed:\n" )
    for fi in range(0,len(failed)):
        summary.write(failed[fi])
        summary.write("\n")
    Rhino.RhinoApp.WriteLine("Not all tests passed! Please check summary file!")

summary.write("\n\n")
summary.close()

for i in benchmarks:
    summary = open('summary.txt', 'a')
    summary.write("%s\n" %(pass_tests[i][0]))
    summary.close()
    
#    