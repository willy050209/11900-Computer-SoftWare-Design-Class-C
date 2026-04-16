import fitz
import json
import re
import sys

def extract_to_file(pdf_path, out_path):
    try:
        doc = fitz.open(pdf_path)
        text = ""
        for page in doc:
            text += page.get_text()
        with open(out_path, 'w', encoding='utf-8') as f:
            f.write(text)
        print(f"Extracted {pdf_path} to {out_path}")
    except Exception as e:
        print(f"Error: {e}")

extract_to_file('學科/119003A13.pdf', '119003A13_raw.txt')
extract_to_file('學科/900060A18.pdf', '900060A18_raw.txt')
