#include <stdio.h>
#include <string.h>
#include <math.h>
#include "cv.h"
#include "highgui.h"

IplImage *img_background;
IplImage *img_p1;
IplImage *img_p2;
IplImage *img_p3;
IplImage *img_p4;

int mpt[4][4] = { 0 };

int main() {
    img_background = cvLoadImage(".//..//background.png");
    img_p1 = cvLoadImage(".//..//1.png");
    img_p2 = cvLoadImage(".//..//2.png");
    img_p3 = cvLoadImage(".//..//3.png");
    img_p4 = cvLoadImage(".//..//4.png");
    cvNamedWindow("pintu", 1);
    mpt[0][0] = 2;
    mpt[1][0] = 1;
    mpt[0][1] = 3;
    while (1) {
        img_background = cvLoadImage(".//..//background.png");
        
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 2; j++) {
                int px = i * 100;
                int py = j * 100;
                if (mpt[i][j] == 1) {
                    for (int x = 0; x < img_p1->height; x++)
                    {
                        for (int y = 0; y < img_p1->width; y++)
                        {

                            int r, g, b;
                            r = CV_IMAGE_ELEM(img_p1, uchar, y, 3 * x);
                            g = CV_IMAGE_ELEM(img_p1, uchar, y, 3 * x + 1);
                            b = CV_IMAGE_ELEM(img_p1, uchar, y, 3 * x + 2);
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px)) = r;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 1) = g;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 2) = b;

                        }
                    }
                }
                else if (mpt[i][j] == 2) {
                    for (int x = 0; x < img_p2->height; x++)
                    {
                        for (int y = 0; y < img_p2->width; y++)
                        {

                            int r, g, b;
                            r = CV_IMAGE_ELEM(img_p2, uchar, y, 3 * x);
                            g = CV_IMAGE_ELEM(img_p2, uchar, y, 3 * x + 1);
                            b = CV_IMAGE_ELEM(img_p2, uchar, y, 3 * x + 2);
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px)) = r;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 1) = g;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 2) = b;

                        }
                    }
                }
                else if (mpt[i][j] == 3) {
                    for (int x = 0; x < img_p3->height; x++)
                    {
                        for (int y = 0; y < img_p3->width; y++)
                        {

                            int r, g, b;
                            r = CV_IMAGE_ELEM(img_p3, uchar, y, 3 * x);
                            g = CV_IMAGE_ELEM(img_p3, uchar, y, 3 * x + 1);
                            b = CV_IMAGE_ELEM(img_p3, uchar, y, 3 * x + 2);
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px)) = r;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 1) = g;
                            CV_IMAGE_ELEM(img_background, uchar, (y + py), 3 * (x + px) + 2) = b;

                        }
                    }
                }
            }
        }
        cvShowImage("pintu", img_background);
        int key = cvWaitKey(100);
        if (key == 'w') {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    if (mpt[i][j] == 0) {
                        x = i;
                        y = j;
                    }
                }
            }
            if (x > 0) {
                int temp = mpt[x][y];
                mpt[x][y] = mpt[x - 1][y];
                mpt[x - 1][y] = temp;
                //swap(mpt[x][y], mpt[x - 1][y]);
            }
        }
        else if (key == 'a') {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    if (mpt[i][j] == 0) {
                        x = i;
                        y = j;
                    }
                }
            }
            if (y > 0) {
                int temp = mpt[x][y];
                mpt[x][y] = mpt[x][y-1];
                mpt[x][y-1] = temp;
                //swap(mpt[x][y], mpt[x - 1][y]);
            }
        }
        else if (key == 's') {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    if (mpt[i][j] == 0) {
                        x = i;
                        y = j;
                    }
                }
            }
            if (x < 1) {
                int temp = mpt[x][y];
                mpt[x][y] = mpt[x + 1][y];
                mpt[x + 1][y] = temp;
                //swap(mpt[x][y], mpt[x - 1][y]);
            }
        }
        else if (key == 'd') {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    if (mpt[i][j] == 0) {
                        x = i;
                        y = j;
                    }
                }
            }
            if (y < 1) {
                int temp = mpt[x][y];
                mpt[x][y] = mpt[x][y + 1];
                mpt[x][y + 1] = temp;
                //swap(mpt[x][y], mpt[x - 1][y]);
            }
        }
    }
    return 0;
}