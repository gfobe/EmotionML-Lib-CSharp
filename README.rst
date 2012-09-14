EmotionML library for C#
========================

With the help of this library you can parse and create `EmotionML <http://www.w3.org/TR/emotionml/>`_ with C#.
Both, EmotionML documents and fragments, are supported.
Furthermore you can work with the represented emotions a little bit.

You can `download this library <https://github.com/gfobe/EmotionML-Lib-CSharp/raw/11120475f4434db3ed5e962b26be03e82d364a9f/dll/EmotionML.dll>`_ in a compiled format.

Using of this library
---------------------
Parsing of EmotionML
^^^^^^^^^^^^^^^^^^^^
You can parse a whole EmotionML document or only parts of it with the build in EmotionML parser.

Load by XmlDocument: ``c#``::
    XmlDocument emotionmlDoc = new XmlDocument();
    emotionmlDoc.Load("your.emotionml");
    Parser emotionmlParser = new Parser(emotionmlDoc);

Load by string: ``c#``::
    Parser emotionmlParser = new Parser(emotionmlString);

After that you can access the parts of EmotionML document you need.

Whole EmotionML document: ``c#``::
    EmotionMLDocument emotionmlDoc = emotionmlParser.getEmotionMLDocument();

List of emotions: ``c#``::
    List<Emotion> emotionList = emotionmlParser.getEmotions();

Single emotion: ``c#``::
    Emotion emotion = emotionmlParser.getSingleEmotion();

List of vocabularies: ``c#``::
    List<Vocabulary> vocabularyList = emotionmlParser.getVocabularies();

Single vocabulary: ``c#``::
    Vocabulary vocabulary = emotionmlParser.getSingleVocabulary();

Creating EmotionML
^^^^^^^^^^^^^^^^^^
You can create XmlDocument instances with the method *ToDom()* and XML with the method *ToXml()*.

Add some emotions to an EmotionML document and output it as XML: ``c#``::
   EmotionMLDocument emodoc = new EmotionMLDocument();
   emodoc.addEmotion(emotion1);
   emodoc.addEmotion(emotion2);
   
   Console.Write(emodoc.ToXml());

Output single emotion as XmlDocument: ``c#``::
   emotion1.ToDom();

Internal structures
-------------------
In general every XML tag has it's own class. 
Furthermore there is a directory *resources* with some XSLT for validation and a file with the default vocabularies of EmotionML. 
This files are accessible via *Helper::loadInternalResource()*. 
However, here is a simplified class diagram:

.. image:: https://raw.github.com/gfobe/EmotionML-Lib-CSharp/12c371608a52035d0195ec45310c16cb0d91530b/doc/class-diagramm-EmotionML-lib_easy.png

Next steps
----------
- integration of automated tests
- references between emotions and vocabularies
- improvements in comparisons

Other things
------------
License: FreeBSD