HtmlScrapper
===========

A library to scrap the blocks of Html.

How it (maybe) will work?
------------

By taking an HTML stream and some XPATHs (or HPATHs?).

Example:
HTML: 
```html
<main>
    <ul>
        <li>One</li>
        <li>Two</li>
        <li>Three</li>
    </ul>
    <div>
        <ul>
            <li>Apple</li>
            <li>Peach</li>
            <li>Watermellon</li>
        </ul>
    </div>    
</main>
```
HPATH: `/**/li`

OUTPUT: `One Two Three Apple Peach Watermellon`.

The example above shows that the Scrapper shouldn't care about the structure, it should detect and extract patterns.

Further output should be nested and have some idea on original structure, so the client code will handle with the output as like it just a stripped HTML (DOM?).