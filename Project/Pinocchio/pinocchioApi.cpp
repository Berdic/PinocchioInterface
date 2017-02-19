/*  This file is part of the Pinocchio automatic rigging library.
    Copyright (C) 2007 Ilya Baran (ibaran@mit.edu)

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

#include "pinocchioApi.h"
#include "debugging.h"
#include <fstream>

ostream *Debugging::outStream = new ofstream();

#define EXPORT extern "C" __declspec(dllexport)

#include <ctime>

int _PHASE = 0;
typedef intptr_t ItemListHandle;


EXPORT bool GenerateItems(ItemListHandle* hItems, double*** itemsFound, int* itemCount, char* file)
{
	clock_t overall = clock();

	clock_t start;

	start = clock();
	_PHASE = 1;
	PinocchioOutput outPut;

	Skeleton humanSkeleton = HumanSkeleton();
	Mesh inputFileMesh = Mesh(file);
	cout << "Starting: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 2;
	//mesh preparing
	Mesh preparedMesh = prepareMesh(inputFileMesh);

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	if (preparedMesh.vertices.size() == 0)
	{
		cout << "EXITED!" << endl;
		_PHASE = -1;
		return false;
	}
	cout << "Mesh preparing: " << clock() - start << endl;

	start = clock();
	_PHASE = 3;
	//construction of distance field
	TreeType *distanceField = constructDistanceField(preparedMesh);
	cout << "constructDistanceField: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 4;
	//discretization
	vector<Sphere> medialSurface = sampleMedialSurface(distanceField);
	cout << "sampleMedialSurface: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 5;
	//sphere packing
	vector<Sphere> spheres = packSpheres(medialSurface);
	cout << "packSpheres: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 6;
	//connecting samples
	PtGraph graph = connectSamples(distanceField, spheres);
	cout << "connectSamples: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 7;
	//computing possibilities
	vector<vector<int> > possibilities = computePossibilities(graph, spheres, humanSkeleton);
	cout << "computePossibilities: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	//constraints can be set by respecifying possibilities for skeleton joints:
	//to constrain joint i to sphere j, use: possiblities[i] = vector<int>(1, j);
	//discrete embeding

	start = clock();
	_PHASE = 8;
	vector<int> embeddingIndices = discreteEmbed(graph, spheres, humanSkeleton, possibilities);


	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	if (embeddingIndices.size() == 0) { //failure
		delete distanceField;
		_PHASE = -1;
		return false;
	}
	cout << "discreteEmbed: " << clock() - start << endl;


	start = clock();
	_PHASE = 9; //Path splitting
	vector<Vector3> discreteEmbedding = splitPaths(embeddingIndices, graph, humanSkeleton);
	cout << "splitPaths: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 10; //medial surface
					//continuous refinement
	vector<Vector3> medialCenters(medialSurface.size());
	for (int i = 0; i < (int)medialSurface.size(); ++i)
		medialCenters[i] = medialSurface[i].center;
	cout << "medialCenters: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 11; //refine embedding
	outPut.embedding = refineEmbedding(distanceField, medialCenters, discreteEmbedding, humanSkeleton);
	cout << "refineEmbedding: " << clock() - start << endl;

	if (_CANCELPROCESS)
	{
		cout << "EXITED!" << endl;
		_PHASE = -2;
		return false;
	}

	start = clock();
	_PHASE = 12; //finishing
	vector<Vector3> done = outPut.embedding;

	/*vector<Vector3> done = std::vector<Vector3>();

	for (int i = 0; i < 500; i++)
	{
	done.push_back(Vector3(i, i + 1, i + 2));
	_progress += 1;

	}*/

	auto items = new std::vector<double*>();

	for (int i = 0; i < done.size(); i++)
	{
		double* extractedVector = new double[3];
		extractedVector[0] = done[i][0];
		extractedVector[1] = done[i][1];
		extractedVector[2] = done[i][2];
		items->push_back(extractedVector);
	}

	*hItems = reinterpret_cast<ItemListHandle>(items);
	*itemsFound = items->data();
	*itemCount = items->size();


	cout << "finishing: " << clock() - start << endl;

	clock_t overallend = clock();

	cout << "Overall time: " << overallend - overall << endl;

	return true;
}

EXPORT bool ReleaseItems(ItemListHandle hItems)
{
	auto items = reinterpret_cast<std::vector<double>*>(hItems);
	delete items;

	return true;
}

EXPORT int GetProgressUpdate()
{
	return _PHASE;
}

EXPORT void CancelProcess()
{
	_CANCELPROCESS = true;
	cout << "CANCELED!";
}

EXPORT void ResetProcess()
{
	_CANCELPROCESS = false;
	_PHASE = 0;
}

PinocchioOutput  autorig(const Skeleton &given, const Mesh &m)
{
    int i;
    PinocchioOutput out;

    Mesh newMesh = prepareMesh(m);

    if(newMesh.vertices.size() == 0)
        return out;

    TreeType *distanceField = constructDistanceField(newMesh);

    //discretization
    vector<Sphere> medialSurface = sampleMedialSurface(distanceField);

    vector<Sphere> spheres = packSpheres(medialSurface);

    PtGraph graph = connectSamples(distanceField, spheres);

    //discrete embedding
    vector<vector<int> > possibilities = computePossibilities(graph, spheres, given);

    //constraints can be set by respecifying possibilities for skeleton joints:
    //to constrain joint i to sphere j, use: possiblities[i] = vector<int>(1, j);

    vector<int> embeddingIndices = discreteEmbed(graph, spheres, given, possibilities);

    if(embeddingIndices.size() == 0) { //failure
        delete distanceField;
        return out;
    }

    vector<Vector3> discreteEmbedding = splitPaths(embeddingIndices, graph, given);

    //continuous refinement
    vector<Vector3> medialCenters(medialSurface.size());
    for(i = 0; i < (int)medialSurface.size(); ++i)
        medialCenters[i] = medialSurface[i].center;

    out.embedding = refineEmbedding(distanceField, medialCenters, discreteEmbedding, given);

    //attachment
    VisTester<TreeType> *tester = new VisTester<TreeType>(distanceField);
    out.attachment = new Attachment(newMesh, given, out.embedding, tester);

    //cleanup
    delete tester;
    delete distanceField;

    return out;
}



