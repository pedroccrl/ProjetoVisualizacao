function ParseUrl(url)
{
    var novo = url.replace(".", "@").replace("/", "#");
    return novo;
}