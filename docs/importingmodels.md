## How can I import my own 3D models?
1. Go to https://poly.google.com/ and upload your model. It supports OBJ and FBX.
2. Make sure to publish your model as PUBLIC or UNLISTED. And mark it "Remixable."
3. Copy the Poly URL, something like https://poly.google.com/view/44l9m6vgUXg
4. In GB, open the Creation Library
5. Click the "Objects from the web" category.
6. Paste your URL into the "Search" text field and press Enter.
7. Click the thumbnail of your model that should show up, and start using it like normal.

You can also add certain hash-tags to your Poly upload description to control importing:

- **#GBPointFilter** will make all textures point-filtered (aka. nearest neighbor). This is best if you want that pixelated look for low-res textures.

- **#GBNoAutoFit** will completely disable any auto-sizing/offsetting that we do. Keep in mind that your model might come in 100x larger or smaller than you'd expect! Try it out, adjust the size/units of your model in your software, then re-upload.

- **#GBTerrainBlock** will auto-scale your block so its max dimension is 2.5m (our blocks are 2.5m wide and 1.5m tall) and place the origin at the bottom-center. This makes it fit exactly with our terrain system. Just remember that you still need to create blocks with the aspect ratio in mind (2.5 / 1.5 = 1.0 / 0.6 ratio).
NOTE: If you add or change tags to existing Poly uploads, they may not immediately work. Wait 15 minutes, type "ClearPolyCache" in the console (~), and try again.

Let us know if you have issues! There are some known issues with transparency, and we're working on fixing that.