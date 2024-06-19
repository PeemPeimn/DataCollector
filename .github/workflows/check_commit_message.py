import re
import sys

regex = "^(build|chore|ci|docs|feat|fix|perf|refactor|revert|style|test){1}(\([\w\-\.]+\))?(!)?: ([\w ])+([\s\S]*)"
txt = sys.argv[1]
match = re.search(regex, txt)

if match is None:
  print("The commit message does not follow the Conventional Commits specification.")
  exit(1)