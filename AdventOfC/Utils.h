#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <sstream>

inline std::vector<std::string> read_all_lines(const std::string& filename)
{
	std::vector<std::string> input;

	std::ifstream file;
	file.open(filename);

	while (!file.eof())
	{
		std::string line;
		getline(file, line);
		input.push_back(line);
	}

	file.close();

	return input;
}

inline std::vector<std::string> split(const std::string& input, const char delimiter)
{
	std::vector<std::string> output;
	std::stringstream data(input);
	std::string element;
	while (std::getline(data, element, delimiter))
	{
		output.push_back(element);
	}
	return output;
}