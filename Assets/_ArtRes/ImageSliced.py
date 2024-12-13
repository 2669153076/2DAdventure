import ImageSliced

#图片切割，使用相同模板

# 读取第一个文件并提取所有的rect
with open('Green-Tree.png.meta', 'r', encoding='utf-8') as file:
    first_file_data = file.read()

# 提取第一个文件中的所有 rect 信息
rect_pattern = re.compile(r'rect:\s*serializedVersion: \d+\s*x: \d+\s*y: \d+\s*width: \d+\s*height: \d+')
first_file_rects = re.findall(rect_pattern, first_file_data)

# 如果第一个文件中没有找到rect信息
if not first_file_rects:
    raise ValueError("在第一个文件中没有找到任何rect信息。")

# 读取第二个文件（目标文件）
with open('Golden-Tree.png.meta', 'r', encoding='utf-8') as file:
    second_file_data = file.read()

# 提取第二个文件中的所有 rect 信息
second_file_rects = re.findall(rect_pattern, second_file_data)

# 检查第二个文件中的 rect 数量与第一个文件中的数量是否匹配
if len(second_file_rects) != len(first_file_rects):
    print(len(second_file_rects))
    print(len(first_file_rects))
    raise ValueError("第一个文件与第二个文件中的rect数量不匹配。")

# 使用第一个文件的 rect 替换第二个文件中的 rect
updated_second_file_data = second_file_data
for first_rect, second_rect in zip(first_file_rects, second_file_rects):
    updated_second_file_data = updated_second_file_data.replace(second_rect, first_rect)

# 保存修改后的内容到新文件
with open('Golden-Tree.png.meta', 'w', encoding='utf-8') as file:
    file.write(updated_second_file_data)

print("所有rect已替换完成，已保存到 second_file_modified.txt")
