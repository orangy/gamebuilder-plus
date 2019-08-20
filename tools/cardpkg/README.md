*Card Packager* is a tool that will produce GB-consumable format from `cards` folder. 

_to be developed_

# Proposal

* Use our own extension to metaJson with the `publishID`, so that file name will not really matter.
* When run on a folder, the tool should go through folders & files recursively and for each found pair of JS & meta
  it will produce a folder in the output directory with the name of the file input file.
  Inside that folder it will put files named `publishID` and `.js` and `.json` extension with the content of original files. 
  This format can be imported by GB and then published to workshop.

# Alternative

* Convince GB devs that it is weird and allow importing directly from our format :)
 