## scraper.py
#import requests
#from bs4 import BeautifulSoup

#url = 'https://quotes.toscrape.com/'
#response = requests.get(url)
#soup = BeautifulSoup(response.text, 'lxml')

#print(soup)
#print ("hello, metanit.com")

with open("parsing_result.prs", "r") as file:
    content = file.read()

print(content)