#pragma once

#include <chrono>
#include <iostream>
#include <ostream>

class timer
{
public:
	timer()
	{
		m_StartTimepoint = std::chrono::high_resolution_clock::now();
	}

	void start()
	{
		m_StartTimepoint = std::chrono::high_resolution_clock::now();
	}

	void stop() const
	{
		const auto end_timepoint = std::chrono::high_resolution_clock::now();
		const auto start = std::chrono::time_point_cast<std::chrono::microseconds>(m_StartTimepoint).time_since_epoch().count();
		const auto end = std::chrono::time_point_cast<std::chrono::microseconds>(end_timepoint).time_since_epoch().count();
		auto duration = end - start;
		double ms = duration * 0.001;

		std::cout << duration << "us (" << ms << "ms)" << std::endl;
	}

private:
	std::chrono::time_point< std::chrono::high_resolution_clock> m_StartTimepoint;
};
