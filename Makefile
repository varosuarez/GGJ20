SHELL := bash
.SHELLFLAGS := -eu -o pipefail -c
.ONESHELL:
.DELETE_ON_ERROR:
MAKEFLAGS += --warn-undefined-variables
MAKEFLAGS += --no-builtin-rules


all: web win linux osx

web: Builds/.web_itch

win: Builds/.win_itch

linux: Builds/.linux_itch

osx : Builds/.osx_itch

Builds/.win_itch: Builds/.win
	$$BUTLER push Builds/Win alvarber/a-bot-2-repair:win
	touch $@

Builds/.linux_itch: Builds/.linux
	$$BUTLER push Builds/Linux alvarber/a-bot-2-repair:linux
	touch $@

Builds/.osx_itch: Builds/.osx
	$$BUTLER push Builds/OSX alvarber/a-bot-2-repair:osx
	touch $@

Builds/.web_itch: Builds/.web
	$$BUTLER push Builds/Web alvarber/a-bot-2-repair:web
	touch $@

Builds/.web: $(shell find Assets -type f | sed 's: :\\ :g') Packages/manifest.json
	"$$UNITY" \
		-batchmode \
		-executeMethod HCF.Build.BuildWeb \
		-logFile /dev/stdout \
		-quit

Builds/.win: $(shell find Assets -type f | sed 's: :\\ :g') Packages/manifest.json
	"$$UNITY" \
		-batchmode \
		-executeMethod HCF.Build.BuildWin \
		-logFile /dev/stdout \
		-quit

Builds/.linux: $(shell find Assets -type f | sed 's: :\\ :g') Packages/manifest.json
	"$$UNITY" \
		-batchmode \
		-executeMethod HCF.Build.BuildLinux \
		-logFile /dev/stdout \
		-quit

Builds/.osx: $(shell find Assets -type f | sed 's: :\\ :g') Packages/manifest.json
	"$$UNITY" \
		-batchmode \
		-executeMethod HCF.Build.BuildOSX \
		-logFile /dev/stdout \
		-quit
