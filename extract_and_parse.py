import fitz
import json
import re
import os

pdf_files = [
    '學科/119003A13.pdf',
    '學科/900060A18.pdf',
    '學科/900070A17.pdf',
    '學科/900080A16.pdf',
    '學科/900090A11.pdf',
    '學科/900110A11.pdf'
]

all_questions = []

for pdf_path in pdf_files:
    if not os.path.exists(pdf_path):
        continue
        
    print(f"Processing {pdf_path}...")
    try:
        doc = fitz.open(pdf_path)
        content = ""
        for page in doc:
            content += page.get_text() + "\n"
    except Exception as e:
        print(f"Error reading {pdf_path}: {e}")
        continue
        
    lines = content.split('\n')
    current_category = "專業科目" if "11900" in pdf_path else "共同科目"
    current_work_item = "未分類"
    
    # regex for matching question start, e.g. "1. (2) " or "1. (2)" or " 1. (2)"
    q_pattern = re.compile(r'^\s*(\d+)\.\s*\(([1-4])\)\s*(.*)')
    
    current_q = None
    
    for line in lines:
        line = line.strip()
        if not line:
            continue
            
        if 'Page ' in line and ' of ' in line:
            continue
            
        if '工作項目' in line:
            m = re.search(r'工作項目\s*\d+\s*[：:]\s*(.*)', line)
            if m:
                current_work_item = m.group(0).strip()
            else:
                current_work_item = line
            continue
            
        match = q_pattern.match(line)
        if match:
            if current_q:
                all_questions.append(current_q)
            
            q_num = int(match.group(1))
            ans = int(match.group(2))
            text = match.group(3)
            
            current_q = {
                "category": current_category,
                "workItem": current_work_item,
                "id": q_num,
                "question": text,
                "options": [],
                "answer": ans
            }
        elif current_q:
            current_q['question'] += " " + line
            
    if current_q:
        all_questions.append(current_q)

# Post-process questions to separate the question body from the options
parsed_questions = []
for q in all_questions:
    text = q['question']
    # split by ①, ②, ③, ④
    opt_pattern = re.split(r'①|②|③|④', text)
    if len(opt_pattern) == 5:
        q['question'] = opt_pattern[0].strip()
        q['options'] = [
            opt_pattern[1].strip(),
            opt_pattern[2].strip(),
            opt_pattern[3].strip(),
            opt_pattern[4].strip().rstrip('。').strip()
        ]
    else:
        # Fallback if splitting fails
        q['options'] = []
    parsed_questions.append(q)

out_file = 'C#/SubjectTestSystem/questions.json'
os.makedirs(os.path.dirname(out_file), exist_ok=True)
with open(out_file, 'w', encoding='utf-8') as f:
    json.dump(parsed_questions, f, ensure_ascii=False, indent=2)

print(f"Extracted and saved {len(parsed_questions)} questions to {out_file}")
