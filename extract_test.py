import fitz
import sys

def check_pdf(path):
    try:
        doc = fitz.open(path)
        text = ""
        for i in range(min(5, len(doc))):
            text += doc[i].get_text()
        print(f"Text from {path} (first 5 pages):")
        print(text[:1000])
    except Exception as e:
        print(f"Error reading {path}: {e}")

check_pdf('學科/119003A13.pdf')
check_pdf('學科/900060A18.pdf')
