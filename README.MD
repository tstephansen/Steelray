# Steelray Software - Coding Assignment

#### Summary
Contains a class which serializes and deserializes an IdTree while retaining the structure.

#### Results
* IdTreeSerializer produces an array that is smaller than the BinaryFormatter.

* IdTreeSerializer deserializes the bytes into an IdTree that is equivalent to the IdTree that was serialized.

#### Notes
* The Serializable attribute was only added to the IdTree and IdNode classes to test against the BinaryFormatter. These attributes are not needed for the IdTreeSerializer to function correctly.